using System;
using System.Collections.Generic;

using static 땅따고.GameDatabase;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace 땅따고
{
    internal static class Extensions
    {
        private static readonly Random Dice = new Random();

        /// <summary>
        ///     점이 존재하는가
        /// </summary>
        /// <param name="점"></param>
        /// <returns>존재하면 true</returns>
        internal static bool 존재하는가(this 점 점)
        {
            if (점 == null)
            {
                return false;
            }

            for (var i = 0; i < 주어진점들.Count; i++)
            {
                if (주어진점들[i] == 점)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     선이 이미 존재하는가
        /// </summary>
        /// <param name="선"></param>
        /// <returns>존재하면 true</returns>
        internal static bool 존재하는가(this 선 선)
        {
            if (선 == null)
            {
                return false;
            }

            for (var i = 0; i < 플레이어들의선들.Count; i++)
            {
                var x = 플레이어들의선들[i];
                if (x == 선)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     선 교차(충돌) 감지 공식 (*선이 겹치거나 만나는것 제외*)
        /// </summary>
        /// <param name="선1"></param>
        /// <param name="선2"></param>
        /// <returns>충돌하면 true</returns>
        internal static bool 교차하는가(this 선 선1, 선 선2)
        {
            if (선1 == null || 선2 == null)
            {
                return false;
            }

            var denominator = (선1.점2.열 - 선1.점1.열) * (선2.점2.행 - 선2.점1.행) - (선1.점2.행 - 선1.점1.행) * (선2.점2.열 - 선2.점1.열);
            var numerator1 = (선1.점1.행 - 선2.점1.행) * (선2.점2.열 - 선2.점1.열) - (선1.점1.열 - 선2.점1.열) * (선2.점2.행 - 선2.점1.행);
            var numerator2 = (선1.점1.행 - 선2.점1.행) * (선1.점2.열 - 선1.점1.열) - (선1.점1.열 - 선2.점1.열) * (선1.점2.행 - 선1.점1.행);

            var r = numerator1 / denominator;
            var s = numerator2 / denominator;

            return r > 0 && r < 1 && s > 0 && s < 1;
        }

        /// <summary>
        ///     점이 점과 점 사이에 있는가 판단하는 공식
        /// </summary>
        /// <param name="점"></param>
        /// <param name="선"></param>
        /// <returns>점이 점과 점 사이에 있으면 true</returns>
        internal static bool 점과점사이에있는가(this 점 점, 선 선)
        {
            if (점 == null || 선 == null)
            {
                return false;
            }

            var dxc = 점.열 - 선.점1.열;
            var dyc = 점.행 - 선.점1.행;

            var dxl = 선.점2.열 - 선.점1.열;
            var dyl = 선.점2.행 - 선.점1.행;

            //이 값이 0이면 같은 각도
            var cross = dxc * dyl - dyc * dxl;

            if (cross != 0)
            {
                return false;
            }

            //점이 점과 점 '사이'에 있는지 확인
            if (Math.Abs(dxl) >= Math.Abs(dyl))
            {
                return dxl > 0 ? 선.점1.열 < 점.열 && 점.열 < 선.점2.열 : 선.점2.열 < 점.열 && 점.열 < 선.점1.열;
            }

            return dyl > 0 ? 선.점1.행 < 점.행 && 점.행 < 선.점2.행 : 선.점2.행 < 점.행 && 점.행 < 선.점1.행;
        }

        /// <summary>
        ///     교차하는 선들을 반환함
        /// </summary>
        /// <param name="선"></param>
        /// <returns>교차하는 선들을 List<선>으로 반환함 없으면 null</returns>
        internal static List<선> 교차하는선구하기(this 선 선)
        {
            var list = new List<선>();

            if (선 == null)
            {
                return list;
            }

            for (var i = 0; i < 플레이어들의선들.Count; i++)
            {
                var x = 플레이어들의선들[i];
                if (x.교차하는가(선))
                {
                    list.Add(x);
                }
            }

            return list;
        }

        /// <summary>
        ///     교차하는 선들의 존재유무를 반환함
        /// </summary>
        /// <param name="선"></param>
        /// <returns>교차하는 선이 있으면 true</returns>
        internal static bool 교차하는선이있는가(this 선 선)
        {
            if (선 == null)
            {
                return false;
            }

            for (var i = 0; i < 플레이어들의선들.Count; i++)
            {
                var x = 플레이어들의선들[i];
                if (x.교차하는가(선))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     선을 구성하는 점과 점 사이에 또 다른 점들을 List<점>으로 반환함
        /// </summary>
        /// <param name="선"></param>
        /// <returns></returns>
        internal static List<점> 사이에있는점구하기(this 선 선)
        {
            var list = new List<점>();

            if (선 == null)
            {
                return list;
            }

            for (var i = 0; i < 주어진점들.Count; i++)
            {
                var x = 주어진점들[i];
                if (x.점과점사이에있는가(선))
                {
                    list.Add(x);
                }
            }

            return list;
        }

        /// <summary>
        ///     선을 구성하는 점과 점 사이에 또 다른 점의 존재유무를 반환함
        /// </summary>
        /// <param name="선"></param>
        /// <returns>점이 있으면 true</returns>
        internal static bool 사이에점이있는가(this 선 선)
        {
            if (선 == null)
            {
                return false;
            }

            for (var i = 0; i < 주어진점들.Count; i++)
            {
                if (주어진점들[i].점과점사이에있는가(선))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     선의 각도가 같은가 확인
        /// </summary>
        /// <param name="선1"></param>
        /// <param name="선2"></param>
        /// <returns>충돌하면 true</returns>
        internal static bool 같은각도인가(this 선 선1, 선 선2)
        {
            if (선1 == null || 선2 == null)
            {
                return false;
            }

            return (선1.점2.열 - 선1.점1.열) * (선2.점2.행 - 선2.점1.행) - (선1.점2.행 - 선1.점1.행) * (선2.점2.열 - 선2.점1.열) == 0;
        }

        /// <summary>
        ///     선이 서로 이어져있는지 확인
        /// </summary>
        /// <param name="선1"></param>
        /// <param name="선2"></param>
        /// <returns>이어져있으면 true</returns>
        internal static bool 이어져있는가(this 선 선1, 선 선2)
        {
            if (선1 == null || 선2 == null)
            {
                return false;
            }

            return 선1.점1 == 선2.점1 || 선1.점1 == 선2.점2 || 선1.점2 == 선2.점1 || 선1.점2 == 선2.점2;
        }

        /// <summary>
        ///     선이 게임 룰을 엄격하게 지키는지 확인
        /// </summary>
        /// <param name="선"></param>
        /// <returns>지키면 true</returns>
        internal static bool 유효한가(this 선 선)
        {
            return 선 != null &&
                   선.점1 != 선.점2 &&
                   !선.존재하는가() &&
                   선.점1.존재하는가() &&
                   선.점2.존재하는가() &&
                   !선.교차하는선이있는가() &&
                   !선.사이에점이있는가();
        }

        /// <summary>
        ///     선들이 삼각형을 구성하는지 확인
        /// </summary>
        /// <param name="선들"></param>
        /// <returns>삼각형이면 true</returns>
        internal static bool 삼각형인가(this List<선> 선들)
        {
            if (선들 == null)
            {
                return false;
            }

            //선이 최소 3개여야 삼각형
            if (선들.Count < 3)
            {
                return false;
            }

            var 큰선들 = new List<선>(); //작은선이 합쳐져서 만들어진 큰선들
            var 작은선들 = new List<선>(); //큰선으로 합쳐져서 삭제해야할 작은선들

            for (var i = 0; i < 선들.Count; i++)
            {
                var 선1 = 선들[i];
                for (var k = 0; k < 선들.Count; k++)
                {
                    var 선2 = 선들[k];
                    if (선2 != 선1 && 선2.이어져있는가(선1) && 선2.같은각도인가(선1))
                    {
                        작은선들.Add(선1);
                        작은선들.Add(선2);

                        var 합쳐진선 = 선1.합치기(선2);
                        var any = false;
                        for (var j = 0; j < 큰선들.Count; j++)
                        {
                            var x = 큰선들[j];
                            if (x == 합쳐진선)
                            {
                                any = true;
                                break;
                            }
                        }

                        if (합쳐진선 != null && !any)
                        {
                            큰선들.Add(선1.합치기(선2));
                        }
                    }
                }
            }

            for (var j = 0; j < 선들.Count; j++)
            {
                var any = false;
                for (var i = 0; i < 작은선들.Count; i++)
                {
                    if (작은선들[i] == 선들[j])
                    {
                        any = true;
                        break;
                    }
                }

                if (any)
                {
                    선들.Remove(선들[j]);
                }
            }

            선들.AddRange(큰선들);

            return 선들.Count == 3 && 삼각형인가(선들[0], 선들[1], 선들[2]);
        }

        /// <summary>
        ///     선 3개가 삼각형을 구성하는지 확인
        /// </summary>
        /// <param name="선1"></param>
        /// <param name="선2"></param>
        /// <param name="선3"></param>
        /// <returns>삼각형이면 true</returns>
        internal static bool 삼각형인가(선 선1, 선 선2, 선 선3)
        {
            if (선1 == null || 선2 == null || 선3 == null)
            {
                return false;
            }

            var 점들 = new List<점>
            {
                선1.점1,
                선1.점2,
                선2.점1,
                선2.점2,
                선3.점1,
                선3.점2
            };

            var count = 0;
            for (var i = 0; i < 점들.Count; i++)
            {
                var x = 점들[i];
                var count1 = 0;
                for (var j = 0; j < 점들.Count; j++)
                {
                    var y = 점들[j];
                    if (y == x)
                    {
                        count1++;
                    }
                }

                if (count1 != 2)
                {
                    count++;
                }
            }

            return count == 0;
        }

        /// <summary>
        ///     선이 어디에있는가를 반환한다.
        /// </summary>
        /// <param name="선1"></param>
        /// <returns></returns>
        internal static 점 선의좌표구하기(this 선 선1)
        {
            return 선1 == null ? null : new 점((선1.점1.행 + 선1.점2.행) / 2, (선1.점1.열 + 선1.점2.열) / 2);
        }

        /// <summary>
        ///     선두개를 합친 선을 반환한다.
        /// </summary>
        /// <param name="선1"></param>
        /// <param name="선2"></param>
        /// <returns></returns>
        internal static 선 합치기(this 선 선1, 선 선2)
        {
            if (선1 == null || 선2 == null)
            {
                return null;
            }

            var 점들 = new List<점>
            {
                선1.점1,
                선1.점2,
                선2.점1,
                선2.점2
            };

            var 중복없는점들 = new List<점>();
            for (var i = 0; i < 점들.Count; i++)
            {
                var count = 0;
                for (var j = 0; j < 점들.Count; j++)
                {
                    if (점들[j] == 점들[i])
                    {
                        count++;
                    }
                }

                if (count == 1)
                {
                    중복없는점들.Add(점들[i]);
                }
            }

            return 중복없는점들.Count != 2 ? null : new 선(중복없는점들[0], 중복없는점들[1]);
        }

        /// <summary>
        ///     문자열의 문자들을 무작위로 섞음
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        internal static string Shuffle(this string str)
        {
            var array = str.ToCharArray();
            var n = array.Length;
            while (n > 1)
            {
                n--;
                var k = Dice.Next(n + 1);
                var value = array[k];
                array[k] = array[n];
                array[n] = value;
            }

            return new string(array);
        }
    }
}