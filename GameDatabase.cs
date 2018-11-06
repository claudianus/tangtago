using System.Collections.Generic;

namespace 땅따고
{
    internal static class GameDatabase
    {
        internal static readonly List<점> 주어진점들 = new List<점>(); //맵파일에서 주어진 점들
        internal static readonly List<선> 모든경우의선 = new List<선>(); //현재맵에서 나올 수 있는 모든 경우의선들
        internal static readonly List<선> 플레이어들의선들 = new List<선>(); //플레이어들이 그은 선들

        internal static int 게임경과;
        internal static 플레이어 누구차례 = 플레이어.Unknwon;

        internal static void 초기화()
        {
            주어진점들.Clear();
            모든경우의선.Clear();
            플레이어들의선들.Clear();
            게임경과 = 0;
            누구차례 = 플레이어.Unknwon;
        }
    }
}