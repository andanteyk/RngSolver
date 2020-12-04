using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngSolver
{
    public static class Invertible
    {
        public static void IsInvertible()
        {

            Console.WriteLine($"start: {DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}");

            using (var context = new Context())
            using (var solver = context.MkSolver())
            {
                var state = Enumerable.Range(0, 3).Select(i => new BitVecWrapper(context.MkBVConst("state" + i, 64), context)).ToArray();

                //*// exists(s0 != s1 && f(s0) == f(s1)) ?

                solver.Assert(state[0].NotEquals(state[1]) as BoolExpr);
                solver.Assert(Output(state[0]).Equals(Output(state[1])) as BoolExpr);

                /*/// exists(s2 _that satisfies_ forall(s0 != s1 => f(s0, s2) != f(s1, s2))) ?

                solver.Assert(context.MkOr(state[0].NotEquals(state[0].MakeConst(0)) as BoolExpr, state[1].NotEquals(state[1].MakeConst(0)) as BoolExpr));
                solver.Assert(context.MkForall(new[] { state[0].Expression as BitVecExpr, state[1].Expression as BitVecExpr }, context.MkImplies(state[0].NotEquals(state[1]) as BoolExpr, Output(state[0], state[2]).NotEquals(Output(state[1], state[2])) as BoolExpr)));

                //*/

                var issat = solver.Check();
                Console.WriteLine(issat);

                if (issat == Status.SATISFIABLE)
                {
                    Console.WriteLine(solver.Model);
                }

            }

            Console.WriteLine($"end  : {DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}");
        }

        public static IArithmetic Output(IArithmetic s)
        {
            return s << 2 ^ s.Sar(19);
        }
        public static IArithmetic Output(IArithmetic s, IArithmetic c)
        {
            return (s + c).Rol(15) - s;
        }
    }
}
