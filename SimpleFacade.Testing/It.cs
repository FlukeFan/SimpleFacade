﻿namespace SimpleFacade.Testing
{
    public static class It
    {
        public static T IsAny<T>()
        {
            return default(T);
        }
    }
}
