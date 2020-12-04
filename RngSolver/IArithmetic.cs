using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngSolver
{
	public abstract class IArithmetic
	{
		public abstract object Expression { get; }

		public abstract IArithmetic MakeConst(ulong x);

		public abstract IArithmetic Identity();
		public abstract IArithmetic Negate();
		public abstract IArithmetic Not();

		public abstract IArithmetic Add(IArithmetic r);
		public abstract IArithmetic Sub(IArithmetic r);
		public abstract IArithmetic Mul(IArithmetic r);
		public abstract IArithmetic MulHi(IArithmetic r);

		public abstract IArithmetic Xor(IArithmetic r);
		public abstract IArithmetic And(IArithmetic r);
		public abstract IArithmetic Or(IArithmetic r);

		public abstract IArithmetic Shl(int sh);
		public abstract IArithmetic Shr(int sh);
		public abstract IArithmetic Sar(int sh);

		public abstract IArithmetic Rol(int sh);
		public abstract IArithmetic Ror(int sh);

		public abstract IArithmetic Shl(IArithmetic sh);
		public abstract IArithmetic Shr(IArithmetic sh);
		public abstract IArithmetic Sar(IArithmetic sh);

		public abstract IArithmetic Rol(IArithmetic sh);
		public abstract IArithmetic Ror(IArithmetic sh);


		public abstract IArithmetic Bswap();

		public abstract object Equals(IArithmetic r);
		public abstract object NotEquals(IArithmetic r);



		public static IArithmetic operator +(IArithmetic u) => u.Identity();
		public static IArithmetic operator -(IArithmetic u) => u.Negate();
		public static IArithmetic operator ~(IArithmetic u) => u.Not();

		public static IArithmetic operator +(IArithmetic l, IArithmetic r) => l.Add(r);
		public static IArithmetic operator +(IArithmetic l, ulong r) => l.Add(l.MakeConst(r));
		public static IArithmetic operator +(ulong l, IArithmetic r) => r.MakeConst(l).Add(r);
		public static IArithmetic operator -(IArithmetic l, IArithmetic r) => l.Sub(r);
		public static IArithmetic operator -(IArithmetic l, ulong r) => l.Sub(l.MakeConst(r));
		public static IArithmetic operator -(ulong l, IArithmetic r) => r.MakeConst(l).Sub(r);
		public static IArithmetic operator *(IArithmetic l, IArithmetic r) => l.Mul(r);
		public static IArithmetic operator *(IArithmetic l, ulong r) => l.Mul(l.MakeConst(r));
		public static IArithmetic operator *(ulong l, IArithmetic r) => r.MakeConst(l).Mul(r);

		public static IArithmetic operator ^(IArithmetic l, IArithmetic r) => l.Xor(r);
		public static IArithmetic operator ^(IArithmetic l, ulong r) => l.Xor(l.MakeConst(r));
		public static IArithmetic operator ^(ulong l, IArithmetic r) => r.MakeConst(l).Xor(r);
		public static IArithmetic operator &(IArithmetic l, IArithmetic r) => l.And(r);
		public static IArithmetic operator &(IArithmetic l, ulong r) => l.And(l.MakeConst(r));
		public static IArithmetic operator &(ulong l, IArithmetic r) => r.MakeConst(l).And(r);
		public static IArithmetic operator |(IArithmetic l, IArithmetic r) => l.Or(r);
		public static IArithmetic operator |(IArithmetic l, ulong r) => l.Or(l.MakeConst(r));
		public static IArithmetic operator |(ulong l, IArithmetic r) => r.MakeConst(l).Or(r);

		public static IArithmetic operator <<(IArithmetic l, int sh) => l.Shl(sh);
		public static IArithmetic operator >>(IArithmetic l, int sh) => l.Shr(sh);

	}
}
