namespace Game.Data
{
    public static partial class Constants
    {
        /// <summary>
        /// Returns the base limit of data of type short equal to - 32767
        /// </summary>
        /// <returns></returns>
        public static short GetBaseLimit()
        {
            return short.MaxValue;
        }

        public static short GetLimitHealth()
        {
            return short.MaxValue;
        }

        public static short GetLimitSpeed()
        {
            return short.MaxValue;
        }

        public static short GetLimitDamage()
        {
            return short.MaxValue;
        }
    }
}