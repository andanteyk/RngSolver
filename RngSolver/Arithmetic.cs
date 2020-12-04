using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngSolver
{
	public class Arithmetic : IArithmetic
	{
		protected readonly ulong expr;

		public Arithmetic(ulong u)
		{
			expr = u;
		}


		protected ulong Eval(IArithmetic x)
		{
			if (x is Arithmetic a)
				return a.expr;
			else
				throw new ArgumentException("type mismatch");
		}

		public override object Expression => expr;

		public override IArithmetic MakeConst(ulong x) => new Arithmetic(x);

		public override IArithmetic Identity() => new Arithmetic(expr);
		public override IArithmetic Negate() => new Arithmetic(0 - expr);
		public override IArithmetic Not() => new Arithmetic(~expr);

		public override IArithmetic Add(IArithmetic r) => new Arithmetic(expr + Eval(r));
		public override IArithmetic Sub(IArithmetic r) => new Arithmetic(expr - Eval(r));
		public override IArithmetic Mul(IArithmetic r) => new Arithmetic(expr * Eval(r));
		public override IArithmetic MulHi(IArithmetic r)
		{
			var mult = Eval(r);
			ulong
				al = (uint)expr,
				ah = expr >> 32,
				bl = (uint)mult,
				bh = mult >> 32;

			ulong
				ll = al * bl,
				lh = al * bh,
				hl = ah * bl,
				hh = ah * bh;

			ulong mid = hl + (ll >> 32) + (uint)lh;
			return new Arithmetic(hh + (mid >> 32) + (lh >> 32));
		}

		public override IArithmetic Xor(IArithmetic r) => new Arithmetic(expr ^ Eval(r));
		public override IArithmetic And(IArithmetic r) => new Arithmetic(expr & Eval(r));
		public override IArithmetic Or(IArithmetic r) => new Arithmetic(expr | Eval(r));

		public override IArithmetic Shl(int sh) => new Arithmetic(expr << sh);
		public override IArithmetic Shr(int sh) => new Arithmetic(expr >> sh);
		public override IArithmetic Sar(int sh) => new Arithmetic((ulong)((long)expr >> sh));

		public override IArithmetic Rol(int sh) => new Arithmetic(expr << sh | expr >> (64 - sh));
		public override IArithmetic Ror(int sh) => new Arithmetic(expr >> sh | expr << (64 - sh));

		public override IArithmetic Shl(IArithmetic sh) => new Arithmetic(expr << (int)(Eval(sh) & 0x3f));
		public override IArithmetic Shr(IArithmetic sh) => new Arithmetic(expr >> (int)(Eval(sh) & 0x3f));
		public override IArithmetic Sar(IArithmetic sh) => new Arithmetic((ulong)((long)expr >> (int)(Eval(sh) & 0x3f)));

		public override IArithmetic Rol(IArithmetic sh) => new Arithmetic(expr << (int)(Eval(sh) & 0x3f) | expr >> (64 - (int)(Eval(sh) & 0x3f)));
		public override IArithmetic Ror(IArithmetic sh) => new Arithmetic(expr >> (int)(Eval(sh) & 0x3f) | expr << (64 - (int)(Eval(sh) & 0x3f)));


		public override IArithmetic Bswap()
		{
			var x = expr;
			x = (x & 0x00ff00ff00ff00ff) << 8 | (x >> 8 & 0x00ff00ff00ff00ff);
			x = (x & 0x0000ffff0000ffff) << 16 | (x >> 16 & 0x0000ffff0000ffff);
			return new Arithmetic(x << 32 | x >> 32);
		}

		public override object Equals(IArithmetic r) => expr == Eval(r);
		public override object NotEquals(IArithmetic r) => expr != Eval(r);


		public override string ToString() => $"{expr}";
	}
}
