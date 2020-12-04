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

        public static void C1133factor()
        {
            uint resolution = 2048;
            using (var context = new Context())
            {
                using (var solver = context.MkSolver())
                {
                    var f = Enumerable.Range(0, 4).Select(i => context.MkBVConst("s" + i, resolution)).ToArray();

                    foreach (var g in f)
                        solver.Assert(context.MkBVSGT(g, context.MkBV(0, resolution)));

                    solver.Assert(context.MkEq(context.MkBVSub(context.MkBVMul(f[0], f[2]), context.MkBVMul(f[1], f[3])), context.MkBV("11512882899820054257144225772505994511430981968359355559240636997087397239461885404688940982112272498773691260355731224763278685518244745544198267923163368736091123701779226072209279679342867029500044275233215203437226071842172804234583591297137729569486761340213325710137879698831126615998659706343950808674850862574868322314902443424081205544133789500128645355501388833990928089030944977862262874243179626287736961093227838096073086612878632276868708056678373714902078426666851025890207418013027573248367464970951431311736356210867866665430397629513384884406535591", resolution)));
                    solver.Assert(context.MkEq(context.MkBVAdd(context.MkBVMul(f[0], f[3]), context.MkBVMul(f[1], f[2])), context.MkBV("200632848085394229198405077309776409669556160755822894920478194045891524675173877582799789843512719390209285348887171584058267613825062519170949236869832740299611688879431491248560122275125138227835639875304442149679485916420376715785002453587853905329008047468218821526665318251417289791164787502264540469658007753188396466487968753988674615092615847790001421479841641921279595503860736218792224235350272376658369292603790019796500735806899786991660195728966759044116399240680328117271881207382080232786405040556863376322477213246700048245459183343930058344600346916", resolution)));

                    var issat = solver.Check();
                    Console.WriteLine(issat);
                    if (issat == Status.SATISFIABLE)
                        Console.WriteLine(solver.Model);
                }
            }

        }

    }
}
