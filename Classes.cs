// ReSharper disable CompareOfFloatsByEqualityOperator


#pragma warning disable 660,661

namespace 땅따고
{
    internal class 선
    {
        internal 점 점1;
        internal 점 점2;
        internal 플레이어 주인;
        internal int 평가;

        internal 선(점 점1, 점 점2)
        {
            this.점1 = 점1;
            this.점2 = 점2;
        }

        internal 선(점 점1, 점 점2, 플레이어 주인)
        {
            this.점1 = 점1;
            this.점2 = 점2;
            this.주인 = 주인;
        }

        internal 선(점 점1, 점 점2, int 평가)
        {
            this.점1 = 점1;
            this.점2 = 점2;
            this.평가 = 평가;
        }

        public static bool operator ==(선 선1, 선 선2)
        {
            if (ReferenceEquals(선1, null))
            {
                return ReferenceEquals(선2, null);
            }

            if (ReferenceEquals(선2, null))
            {
                return false;
            }

            return 선1.점1 == 선2.점1 && 선1.점2 == 선2.점2 || 선1.점1 == 선2.점2 && 선1.점2 == 선2.점1;
        }

        public static bool operator !=(선 선1, 선 선2)
        {
            return !(선1 == 선2);
        }

        public override string ToString()
        {
#if DEBUG
            return $"{점1}-{점2}";
#else
            return $"{점1} {점2}";
#endif
        }
    }

    internal class 점
    {
        internal float 열;
        internal float 행;

        internal 점(float 행, float 열)
        {
            this.행 = 행;
            this.열 = 열;
        }

        public static bool operator ==(점 점1, 점 점2)
        {
            if (ReferenceEquals(점1, null))
            {
                return ReferenceEquals(점2, null);
            }

            if (ReferenceEquals(점2, null))
            {
                return false;
            }

            return 점1.열 == 점2.열 && 점1.행 == 점2.행;
        }

        public static bool operator !=(점 점1, 점 점2)
        {
            return !(점1 == 점2);
        }

        public override string ToString()
        {
#if DEBUG
            return $"({행},{열})";
#else
            return $"{행} {열}";
#endif
        }
    }
}