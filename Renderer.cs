using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using static 땅따고.Config;
using static 땅따고.GameDatabase;

// ReSharper disable PossibleLossOfFraction

namespace 땅따고
{
    internal static class Renderer
    {
        private const float 블록가로길이 = (비트맵가로해상도 - 비트맵가장자리여백 * 2) / 보드가로블록갯수;
        private const float 블록세로길이 = (비트맵세로해상도 - 비트맵가장자리여백 * 2) / 보드세로블록갯수;

        private static readonly Font 점좌표문자열폰트 = new Font(점좌표글꼴,
                                                         점좌표글꼴크기,
                                                         FontStyle.Bold,
                                                         GraphicsUnit.Pixel);

        private static readonly Font 점수판폰트 = new Font(점수판글꼴,
                                                      점수판글꼴크기,
                                                      FontStyle.Bold,
                                                      GraphicsUnit.Pixel);

        private static readonly Font 선순서폰트 = new Font(선순서글꼴,
                                                      선순서글꼴크기,
                                                      FontStyle.Bold,
                                                      GraphicsUnit.Pixel);

        private static readonly StringFormat 문자열센터포맷 = new StringFormat
        {
            Alignment = StringAlignment.Center,
            LineAlignment = StringAlignment.Center
        };

        private static readonly Pen 블록윤곽선펜 = new Pen(Color.DarkGray) {Width = 블록윤곽선두께};
        private static readonly Pen 보드윤곽선펜 = new Pen(Color.Black) {Width = 보드윤곽선두께};

        /// <summary>
        ///     게임 상황 렌더링
        /// </summary>
        /// <returns></returns>
        internal static bool Render(string filename, bool open = false)
        {
            try
            {
                using (var b = new Bitmap(비트맵가로해상도, 비트맵세로해상도))
                {
                    using (var g = Graphics.FromImage(b))
                    {
                        if (비트맵안티얼라이어싱)
                        {
                            g.SmoothingMode = SmoothingMode.AntiAlias;
                        }
                        g.Clear(Color.LightGray);

                        //블록 그리기
                        for (var i = 0; i < 보드세로블록갯수; i++)
                        for (var j = 0; j < 보드가로블록갯수; j++)
                        {
                            //각 블록에 패턴 색칠하기
                            if (i % 2 == 0)
                            {
                                g.FillRectangle(j % 2 != 0 ? Brushes.LightGray : Brushes.WhiteSmoke,
                                                비트맵가장자리여백 + j * 블록가로길이,
                                                비트맵가장자리여백 + i * 블록세로길이,
                                                블록가로길이,
                                                블록세로길이);
                            }
                            else
                            {
                                g.FillRectangle(j % 2 == 0 ? Brushes.LightGray : Brushes.WhiteSmoke,
                                                비트맵가장자리여백 + j * 블록가로길이,
                                                비트맵가장자리여백 + i * 블록세로길이,
                                                블록가로길이,
                                                블록세로길이);
                            }

                            //각 블록에 테두리 그리기                                
                            g.DrawRectangle(블록윤곽선펜,
                                            비트맵가장자리여백 + j * 블록가로길이,
                                            비트맵가장자리여백 + i * 블록세로길이,
                                            블록가로길이,
                                            블록세로길이);
                        }

                        //보드 윤곽선 그리기
                        g.DrawRectangle(보드윤곽선펜,
                                        비트맵가장자리여백,
                                        비트맵가장자리여백,
                                        비트맵가로해상도 - 비트맵가장자리여백 * 2,
                                        비트맵세로해상도 - 비트맵가장자리여백 * 2);

                        //플레이어 선들 그리기
                        for (var i = 0; i < 플레이어들의선들.Count; i++)
                        {
                            var 선 = 플레이어들의선들[i];
                            Color 선의색;

                            switch (선.주인)
                            {
                                case 플레이어.땅따고:
                                    선의색 = Color.Red;
                                    break;
                                case 플레이어.도전자:
                                    선의색 = Color.Blue;
                                    break;
                                default:
                                    선의색 = Color.Gray;
                                    break;
                            }

                            var thePen = new Pen(선의색)
                            {
                                Width = 플레이어선두께
                            };
                            g.DrawLine(thePen,
                                       비트맵가장자리여백 + 선.점1.열 * 블록세로길이,
                                       비트맵가장자리여백 + 선.점1.행 * 블록가로길이,
                                       비트맵가장자리여백 + 선.점2.열 * 블록세로길이,
                                       비트맵가장자리여백 + 선.점2.행 * 블록가로길이);

                            //선순서 그리기
                            var 선좌표 = 선.선의좌표구하기();
                            g.DrawString((i + 1).ToString(),
                                         선순서폰트,
                                         Brushes.Black,
                                         선좌표.열 * 블록가로길이 + 비트맵가장자리여백 - 선순서글꼴크기 / 2,
                                         선좌표.행 * 블록세로길이 + 비트맵가장자리여백 - 선순서글꼴크기 / 2);
                        }

                        //주어진점들 그리기
                        foreach (var 점 in 주어진점들)
                        {
                            g.DrawString($"{점}",
                                         점좌표문자열폰트,
                                         Brushes.Black,
                                         비트맵가장자리여백 + 점.열 * 블록가로길이,
                                         비트맵가장자리여백 + 점.행 * 블록세로길이 - (점좌표글꼴크기 + 1),
                                         문자열센터포맷);
                            g.FillEllipse(Brushes.Black,
                                          비트맵가장자리여백 + 점.열 * 블록가로길이 - 점크기 / 2,
                                          비트맵가장자리여백 + 점.행 * 블록세로길이 - 점크기 / 2,
                                          점크기,
                                          점크기);
                        }

                        //땅따고, 도전자 그리기
                        g.DrawString("땅따고",
                                     점수판폰트,
                                     Brushes.Red,
                                     비트맵가로해상도 / 2 - 블록가로길이 / 2,
                                     비트맵세로해상도 / 2 - 블록세로길이 / 2,
                                     문자열센터포맷);

                        g.DrawString("도전자",
                                     점수판폰트,
                                     Brushes.Blue,
                                     비트맵가로해상도 / 2 + 블록가로길이 / 2,
                                     비트맵세로해상도 / 2 - 블록세로길이 / 2,
                                     문자열센터포맷);
                    }

                    b.Save(filename, ImageFormat.Png);
                    if (open)
                    {
                        Process.Start("explorer", filename);
                    }
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}