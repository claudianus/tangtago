using System;
using System.IO;
using System.Linq;

using static 땅따고.ConsoleEx;
using static 땅따고.Config;
using static 땅따고.GameDatabase;
using static 땅따고.Renderer;
using static 땅따고.Utilities;

namespace 땅따고
{
    internal static class Game
    {
        /// <summary>
        ///     게임시작
        /// </summary>
        /// <param name="mapfile"></param>
        /// <returns>게임이 정상적으로 끝나면 true</returns>
        internal static bool BeginGame(string mapfile)
        {
#if DEBUG
            PrintLine("[*] 준비중...\n");
#endif

            초기화();

            var mapString = string.Empty;
#if DEBUG //디버그모드일경우 무작위로 맵을 생성함
            mapString = 무작위맵생성(60);
#else //릴리즈모드일경우 map.txt파일에서 맵을 불러옴
//파일에서 맵을 불러옴
            try
            {
                mapString = File.ReadAllText(mapfile);
            }
            catch
            {
                PrintLine("[!] 맵을 로드하지 못 했습니다.", ConsoleColor.Red);
                Console.Beep();
                Console.ReadLine();
                return false;
            }

            if (mapString.Length < (보드가로블록갯수 + 1) * (보드세로블록갯수 + 1))
            {
                PrintLine("[!] 맵 파일이 호환되지않습니다.", ConsoleColor.Red);
                Console.Beep();
                Console.ReadLine();
                return false;
            }
#endif

            //주어진 점들 캐싱
            var stringPointer = 0;
            for (var i = 0; i < 보드세로블록갯수 + 1; i++)
            for (var j = 0; j < 보드가로블록갯수 + 1; j++)
            {
                switch (mapString[stringPointer])
                {
                    case '0':
                        break;
                    case '1':
                        주어진점들.Add(new 점(i, j));
                        break;
                }

                stringPointer++;
            }

            //모든경우의 선들 캐싱
            foreach (var 점1 in 주어진점들)
            {
                foreach (var 점2 in 주어진점들.Where(x => x != 점1))
                {
                    var 선 = new 선(점1, 점2);
                    if (모든경우의선.All(x => x != 선))
                    {
                        모든경우의선.Add(선);
                    }
                }
            }

            //게임기록 폴더 생성
            var historyDirectory =
                $"{Environment.CurrentDirectory}\\{DateTime.Now.Year}년{DateTime.Now.Month}월{DateTime.Now.Day}일{DateTime.Now.Hour}시{DateTime.Now.Minute}분{DateTime.Now.Second}초 기록";
            Directory.CreateDirectory(historyDirectory);

            //맵 그리기
            Render($"{historyDirectory}\\0 맵.png");

#if DEBUG //땅따고 선공 세팅
            누구차례 = 플레이어.땅따고;
#else //도전자 선공 세팅
            누구차례 = 플레이어.도전자;
#endif
            게임경과 = 1;
            while (true)
            {
                switch (누구차례)
                {
                    case 플레이어.땅따고:

                        //땅따고가 공격
                        var 최고의선 = Intelligence.선선택();
                        플레이어들의선들.Add(최고의선);
#if DEBUG
                        PrintLine($"[*] 땅따고가 선 {최고의선}을 그었습니다.\n", ConsoleColor.Red);
#else
                        PrintLine($"{최고의선}", ConsoleColor.Red);
#endif

                        break;
                    case 플레이어.도전자:

                        //도전자가 공격
#if DEBUG
                        PrintLine("[*] 당신이 공격할 차례입니다.", ConsoleColor.Cyan);
                        PrintLine("[*] 선을 이을 두점의 좌표를 예시와 같게 입력해주세요. 예) 0 0 1 1.");
                        PrintLine("[*] 0 0 0 0입력시 땅따고에게 차례를 넘깁니다.");
#endif

                        var input = Console.ReadLine();
                        var split = input.Split(' ');

                        //입력한 값이 제데로된 포맷인지 확인
                        if (split.Length != 4)
                        {
                            PrintLine("[!] 잘못된 포맷을 입력했습니다. 올바른 예) 0 0 1 1", ConsoleColor.Red);
                            Console.Beep();
                            continue;
                        }

                        //입력한 값이 정수인지 아닌지 확인
                        var isNotInt = false;
                        foreach (var item in split)
                        {
                            try
                            {
                                int.Parse(item);
                            }
                            catch
                            {
                                isNotInt = true;
                                break;
                            }
                        }

                        if (isNotInt)
                        {
                            PrintLine("[*] 정수만 입력하십시오.", ConsoleColor.Red);
                            Console.Beep();
                            continue;
                        }

                        //차례를 넘기는지 확인
                        if (input == "0 0 0 0")
                        {
                            //땅따고 차례로 전환
                            누구차례 = 플레이어.땅따고;
#if DEBUG
                            PrintLine("[*] 땅따고에게 차례를 넘깁니다.", ConsoleColor.Magenta);
#endif

                            //Console.Beep();
                            continue;
                        }

                        var 점1 = new 점(int.Parse(split[0]), int.Parse(split[1]));
                        var 점2 = new 점(int.Parse(split[2]), int.Parse(split[3]));

                        //같은 좌표를 입력했는지 확인
                        if (점1 == 점2)
                        {
                            PrintLine("[!] 같은 좌표를 입력했습니다.", ConsoleColor.Red);
                            Console.Beep();
                            continue;
                        }

                        //존재하는점을 입력했는지 확인
                        점[] 점들 = {점1, 점2};
                        var 존재하지않는점들 = 점들.Where(x => 주어진점들.All(y => y != x)).ToList();
                        if (존재하지않는점들.Count > 0)
                        {
                            Print($"[!] 입력하신 점 ", ConsoleColor.Red);
                            for (var i = 0; i < 존재하지않는점들.Count; i++)
                            {
                                Print(존재하지않는점들[i].ToString());

                                if (i + 1 < 존재하지않는점들.Count)
                                {
                                    Print(", ");
                                }
                            }

                            Print("(들)은 존재하지않습니다.\n", ConsoleColor.Red);
                            Console.Beep();
                            continue;
                        }

                        var 선 = new 선(점1, 점2, 플레이어.도전자);

                        //존재하지않는 점을 입력했는지 확인
                        if (선.존재하는가())
                        {
                            PrintLine($"[!] 선 {선}은 이미 존재합니다.", ConsoleColor.Red);
                            Console.Beep();
                            continue;
                        }

                        //교차하지 않는 선을 입력했는지 확인
                        var 교차하는선들 = 선.교차하는선구하기();
                        if (교차하는선들.Any())
                        {
                            Print($"[!] 입력하신 선을 그리면 선 ", ConsoleColor.Red);
                            for (var i = 0; i < 교차하는선들.Count; i++)
                            {
                                Print(교차하는선들[i].ToString());

                                if (i + 1 < 교차하는선들.Count)
                                {
                                    Print(", ");
                                }
                            }

                            Print("(들)과 교차합니다.\n\n", ConsoleColor.Red);
                            Console.Beep();
                            continue;
                        }

                        //점을 통과하지않는 선을 입력했는지 확인
                        var 선사이에있는점들 = 선.사이에있는점구하기();
                        if (선사이에있는점들.Any())
                        {
                            Print($"[!] 입력하신 선을 그리면 선이 점 ", ConsoleColor.Red);
                            for (var i = 0; i < 선사이에있는점들.Count; i++)
                            {
                                Print(선사이에있는점들[i].ToString());

                                if (i + 1 < 선사이에있는점들.Count)
                                {
                                    Print(", ");
                                }
                            }

                            Print("(들)을 통과합니다.\n", ConsoleColor.Red);
                            Console.Beep();
                            continue;
                        }

                        플레이어들의선들.Add(선);
#if DEBUG
                        PrintLine($"[*] {선} 당신이 선을 그었습니다.\n", ConsoleColor.Blue);
#endif
                        break;
                }

                //게임 상황 그리기
                Render($"{historyDirectory}\\{게임경과} {누구차례}가 공격함.png");

                //게임이 끝났는지 확인
                if (게임이끝났는가())
                {
                    //게임이 끝남
                    PrintLine("[!] 게임이 끝났습니다.");
                    Console.Beep();
                    Console.ReadLine();
                    break;
                }

                게임경과++;
                switch (누구차례)
                {
                    case 플레이어.도전자:
                        누구차례 = 플레이어.땅따고;
                        break;
                    case 플레이어.땅따고:
                        누구차례 = 플레이어.도전자;
                        break;
                }

#if DEBUG
                누구차례 = 플레이어.땅따고;
#endif
            }

            return true;
        }
    }
}