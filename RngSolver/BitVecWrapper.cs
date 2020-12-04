using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngSolver
{
	public class BitVecWrapper : IArithmetic
	{
		protected readonly BitVecExpr expr;
		protected uint bits;
		protected readonly Context context;

		public BitVecWrapper(BitVecExpr b, uint bits, Context c)
		{
			expr = b;
			context = c;
			this.bits = bits;
		}
		public BitVecWrapper(BitVecExpr b, Context c) : this(b, b.SortSize, c) { }


		protected BitVecExpr Eval(IArithmetic x)
		{
			if (x is BitVecWrapper a)
				return a.expr;
			else
				throw new ArgumentException("type mismatch");
		}

		public override object Expression => expr;
		public override IArithmetic MakeConst(ulong x) => new BitVecWrapper(context.MkBV(x, 64), 64, context);

		public override IArithmetic Identity() => new BitVecWrapper(expr, context);
		public override IArithmetic Negate() => new BitVecWrapper(context.MkBVNeg(expr), context);
		public override IArithmetic Not() => new BitVecWrapper(context.MkBVNot(expr), context);

		public override IArithmetic Add(IArithmetic r) => new BitVecWrapper(context.MkBVAdd(expr, Eval(r)), context);
		public override IArithmetic Sub(IArithmetic r) => new BitVecWrapper(context.MkBVSub(expr, Eval(r)), context);
		public override IArithmetic Mul(IArithmetic r) => new BitVecWrapper(context.MkBVMul(expr, Eval(r)), context);
		public override IArithmetic MulHi(IArithmetic r)
		{
			var a = context.MkZeroExt(expr.SortSize * 2, expr);
			var basemult = Eval(r);
			var b = context.MkZeroExt(basemult.SortSize * 2, basemult);

			var multed = context.MkBVMul(a, b);
			return new BitVecWrapper(context.MkExtract(expr.SortSize * 2 - 1, expr.SortSize, multed), context);
		}

		public override IArithmetic Xor(IArithmetic r) => new BitVecWrapper(context.MkBVXOR(expr, Eval(r)), context);
		public override IArithmetic And(IArithmetic r) => new BitVecWrapper(context.MkBVAND(expr, Eval(r)), context);
		public override IArithmetic Or(IArithmetic r) => new BitVecWrapper(context.MkBVOR(expr, Eval(r)), context);

		public override IArithmetic Shl(int sh) => new BitVecWrapper(context.MkBVSHL(expr, context.MkBV(sh, bits)), context);
		public override IArithmetic Shr(int sh) => new BitVecWrapper(context.MkBVLSHR(expr, context.MkBV(sh, bits)), context);
		public override IArithmetic Sar(int sh) => new BitVecWrapper(context.MkBVASHR(expr, context.MkBV(sh, bits)), context);

		public override IArithmetic Rol(int sh) => new BitVecWrapper(context.MkBVRotateLeft((uint)sh, expr), context);
		public override IArithmetic Ror(int sh) => new BitVecWrapper(context.MkBVRotateRight((uint)sh, expr), context);

		public override IArithmetic Shl(IArithmetic sh) => new BitVecWrapper(context.MkBVSHL(expr, Eval(sh)), context);
		public override IArithmetic Shr(IArithmetic sh) => new BitVecWrapper(context.MkBVLSHR(expr, Eval(sh)), context);
		public override IArithmetic Sar(IArithmetic sh) => new BitVecWrapper(context.MkBVASHR(expr, Eval(sh)), context);

		public override IArithmetic Rol(IArithmetic sh) => new BitVecWrapper(context.MkBVRotateLeft(expr, Eval(sh)), context);
		public override IArithmetic Ror(IArithmetic sh) => new BitVecWrapper(context.MkBVRotateRight(expr, Eval(sh)), context);

		public override IArithmetic Bswap()
		{
			if (bits != 64)
				throw new InvalidOperationException("bits != 64");

			var mask8 = context.MkBV(0x00ff00ff00ff00ff, 64);
			var mask16 = context.MkBV(0x0000ffff0000ffff, 64);
			var sh8 = context.MkBV(8, 64);
			var x = expr;
			x = context.MkBVOR(
				context.MkBVSHL(context.MkBVAND(x, mask8), sh8),
				context.MkBVAND(context.MkBVLSHR(x, sh8), mask8)
				);
			x = context.MkBVOR(
				context.MkBVRotateRight(16, context.MkBVAND(x, mask16)),
				context.MkBVAND(context.MkBVRotateLeft(16, x), mask16)
				);
			return new BitVecWrapper(x, context);
		}

		public override object Equals(IArithmetic r)
		{
			if (r is BitVecWrapper x)
				return context.MkEq(expr, x.expr);
			else if (r is Arithmetic a)
				return context.MkEq(expr, context.MkBV((ulong)a.Expression, bits));
			else
				throw new NotSupportedException();
		}
		public override object NotEquals(IArithmetic r) => context.MkNot(Equals(r) as BoolExpr);


		public override string ToString() => $"{expr}";
	}
}
