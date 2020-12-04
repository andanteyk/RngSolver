using Microsoft.Z3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RngSolver
{
    public static class SolveRng
    {
        public static void Solve()
        {
            var seed = new ulong[] { 0x0123456789abcdef, 0xfedcba9876543210 }
              .Select(s => new Arithmetic(s)).ToArray();

            var outputs = new Arithmetic[3];
            {
                var clone = seed.Select(s => s.Identity()).ToArray();
                for (int i = 0; i < outputs.Length; i++)
                    outputs[i] = Next(clone) as Arithmetic;
            }


            Console.WriteLine($"start: {DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}");

            using (var context = new Context())
            using (var solver = context.MkSolver())
            {
                var state = Enumerable.Range(0, seed.Length).Select(i => new BitVecWrapper(context.MkBVConst("state" + i, 64), context)).ToArray();

                for (int i = 0; i < outputs.Length; i++)
                {
                    var res = Next(state);
                    solver.Assert(res.Equals(outputs[i]) as BoolExpr);
                }


                var issat = solver.Check();
                Console.WriteLine(issat);

                if (issat == Status.SATISFIABLE)
                {
                    Console.WriteLine(solver.Model);
                }

            }

            Console.WriteLine($"end  : {DateTime.Now:yyyy/MM/dd HH:mm:ss.fff}");
        }


        public static IArithmetic Next(IArithmetic[] s)
        {
            var s0 = s[0];
            var s1 = s[1];

            var result = ((s0 + s1) * 9).Rol(29) + s0;

            s[0] = s0 ^ s1.Rol(29);
            s[1] = s0 ^ s1 << 9;

            return result;
        }
    }
}
