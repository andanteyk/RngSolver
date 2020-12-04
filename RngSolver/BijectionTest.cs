using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngSolver
{
    class BijectionTest
    {
        public static void TestBijection()
        {
            using (var context = new Context())
            {
                for (uint i = 0; i < 64; i++)
                {
                    using (var solver = context.MkSolver())
                    {
                        var r0 = context.MkBVConst("r0", 64);
                        var r1 = context.MkBVConst("r1", 64);

                        BitVecExpr poke(BitVecExpr expr) => context.MkBVAdd(context.MkBVRotateLeft(i, expr), expr);

                        var a0 = poke(r0);
                        var a1 = poke(r1);

                        solver.Assert(context.MkNot(context.MkEq(r0, r1)));
                        solver.Assert(context.MkEq(a0, a1));


                        var issat = solver.Check();
                        Console.WriteLine(i + " : " + issat);
                        if (issat == Status.SATISFIABLE)
                            Console.WriteLine(solver.Model);
                    }
                }
            }
        }

    }
}
