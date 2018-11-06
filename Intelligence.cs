using System;
using System.Diagnostics;

using static 땅따고.Extensions;
using static 땅따고.GameDatabase;
using static 땅따고.Utilities;
using static 땅따고.ConsoleEx;

namespace 땅따고
{
    internal static class Intelligence
    {
        private static readonly Random Dice = new Random();

        /// <summary>
        ///     인공지능이 선택한 선을 반환함.
        ///     알고리즘
        ///     그으면 삼각형이 되는 선을 찾음.
        ///         ㄴ 찾지 못 했을 경우 상대가 삼각형을 만들지 못하는 선을 찾음
        ///             ㄴ 찾지 못 했을 경우 무작위 선을 반환함.
        ///     *
        ///     여러개의 선으로 삼각형을 완성하는쪽이 오히려 점수에서 불리할것으로 가정하여
        ///     여러개의 선으로 삼각형을 만들 수 있다는 사실은 무시함.
        /// </summary>
        /// <returns></returns>
        internal static 선 선선택()
        {
#if DEBUG
            var stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            선 최고의선 = null;

            var 유효한선들 = 유효한선구하기();

            //그으면 삼각형이 되는 선을 찾음
            for (var i = 0; i < 유효한선들.Count; i++)
            {
                var 유효한선 = 유효한선들[i];
                for (var j = 0; j < 플레이어들의선들.Count; j++)
                {
                    var 선1 = 플레이어들의선들[j];
                    for (var k = 0; k < 플레이어들의선들.Count; k++)
                    {
                        var 선2 = 플레이어들의선들[k];
                        if (선2 != 선1 && 삼각형인가(유효한선, 선1, 선2))
                        {
#if DEBUG
                            Console.WriteLine($"삼각형을 만드는선: {유효한선}");
#endif
                            최고의선 = 유효한선;
                            break;
                        }
                    }

                    if (최고의선 != null)
                    {
                        break;
                    }
                }

                if (최고의선 != null)
                {
                    break;
                }
            }

            if (최고의선 == null)
            {
                //단선 긋기
                for (var i = 0; i < 유효한선들.Count; i++)
                {
                    var x = 유효한선들[i];
                    var any = false;
                    for (var j = 0; j < 플레이어들의선들.Count; j++)
                    {
                        var y = 플레이어들의선들[j];
                        if (y.이어져있는가(x))
                        {
                            any = true;
                            break;
                        }
                    }

                    if (!any)
                    {
#if DEBUG
                        Console.WriteLine($"단선: {x}");
#endif
                        최고의선 = x;
                        break;
                    }
                }
            }

            if (최고의선 == null)
            {
                //다음턴에 상대가 삼각형을 만들수없게되는 선을 찾음.
                foreach (var 유효한선 in 유효한선들)
                {
                    var any = false;
                    foreach (var 예측된상대의선 in 유효한선들)
                    {
                        foreach (var 플레이어의선1 in 플레이어들의선들)
                        {
                            if (삼각형인가(유효한선, 예측된상대의선, 플레이어의선1))
                            {
                                any = true;
                                break;
                            }
                        }

                        if (any)
                        {
                            break;
                        }
                    }

                    if (!any)
                    {
#if DEBUG
                        Console.WriteLine($"상대에게불리한선: {유효한선}");
#endif
                        최고의선 = 유효한선;
                        break;
                    }
                }
            }

            if (최고의선 == null)
            {
                //무작위선 반환
                최고의선 = 유효한선들[Dice.Next(0, 유효한선들.Count)];
#if DEBUG
                Console.WriteLine($"무작위선: {최고의선}");
#endif
            }

            최고의선.주인 = 플레이어.땅따고;
#if DEBUG
            stopwatch.Stop();
            PrintLine($"선 결정까지 걸린시간: {stopwatch.ElapsedMilliseconds}ms");
#endif
            return 최고의선;
        }
    }
}