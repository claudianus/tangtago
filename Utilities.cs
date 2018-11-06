using System.Collections.Generic;

using static 땅따고.GameDatabase;
using static 땅따고.Config;

namespace 땅따고
{
    internal static class Utilities
    {
        /// <summary>
        ///     현재 그을 수 있는 모든 선을 반환한다.
        /// </summary>
        /// <returns></returns>
        internal static List<선> 유효한선구하기()
        {
            var list = new List<선>();
            for (var i = 0; i < 모든경우의선.Count; i++)
            {
                var x = 모든경우의선[i];
                if (x.유효한가())
                {
                    list.Add(x);
                }
            }

            return list;
        }

        /// <summary>
        ///     더 이상 선을 그릴 수 없는지 확인
        /// </summary>
        /// <returns>더 이상 그릴 수 없으면 true</returns>
        internal static bool 게임이끝났는가()
        {
            for (var i = 0; i < 모든경우의선.Count; i++)
            {
                if (모든경우의선[i].유효한가())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     무작위로 맵을 생성함
        /// </summary>
        /// <param name="point">점갯수</param>
        /// <returns></returns>
        internal static string 무작위맵생성(int point)
        {
            var buffer = string.Empty;

            for (var i = 0; i < (보드가로블록갯수 + 1) * (보드세로블록갯수 + 1); i++)
            {
                buffer += i < point ? "1" : "0";
            }

            return buffer.Shuffle();
        }
    }
}