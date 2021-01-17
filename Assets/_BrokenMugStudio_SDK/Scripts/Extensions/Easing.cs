using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hawkeen.Extentions
{
    public class Easing
    {
        public enum Type { Linear,
            QuadraticIn, QuadraticOut, QuadraticInOut,
            CubicIn, CubicOut, CubicInOut,
            QuarticIn, QuarticOut, QuarticInOut,
            QuinticIn, QuinticOut, QuinticInOut,
            SinusoidalIn, SinusoidalOut, SinusoidalInOut,
            ExponentialIn, ExponentialOut, ExponentialInOut,
            CircularIn, CircularOut, CircularInOut,
            ElasticIn, ElasticOut, ElasticInOut,
            BackIn, BackOut, BackInOut,
            BounceIn, BounceOut, BounceInOut
        }

        public static float Ease(Type i_Type, float k)
        {
            switch (i_Type)
            {
                case Type.Linear:
                    return Linear(k);
                case Type.QuadraticIn:
                    return Quadratic.In(k);
                case Type.QuadraticOut:
                    return Quadratic.Out(k);
                case Type.QuadraticInOut:
                    return Quadratic.InOut(k);
                case Type.CubicIn:
                    return Cubic.In(k);
                case Type.CubicOut:
                    return Cubic.Out(k);
                case Type.CubicInOut:
                    return Cubic.InOut(k);
                case Type.QuarticIn:
                    return Quartic.In(k);
                case Type.QuarticOut:
                    return Quartic.Out(k);
                case Type.QuarticInOut:
                    return Quartic.InOut(k);
                case Type.QuinticIn:
                    return Quintic.In(k);
                case Type.QuinticOut:
                    return Quintic.Out(k);
                case Type.QuinticInOut:
                    return Quintic.InOut(k);
                case Type.SinusoidalIn:
                    return Sinusoidal.In(k);
                case Type.SinusoidalOut:
                    return Sinusoidal.Out(k);
                case Type.SinusoidalInOut:
                    return Sinusoidal.InOut(k);
                case Type.ExponentialIn:
                    return Exponential.In(k);
                case Type.ExponentialOut:
                    return Exponential.Out(k);
                case Type.ExponentialInOut:
                    return Exponential.InOut(k);
                case Type.CircularIn:
                    return Circular.In(k);
                case Type.CircularOut:
                    return Circular.Out(k);
                case Type.CircularInOut:
                    return Circular.InOut(k);
                case Type.ElasticIn:
                    return Elastic.In(k);
                case Type.ElasticOut:
                    return Elastic.Out(k);
                case Type.ElasticInOut:
                    return Elastic.InOut(k);
                case Type.BackIn:
                    return Back.In(k);
                case Type.BackOut:
                    return Back.Out(k);
                case Type.BackInOut:
                    return Back.InOut(k);
                case Type.BounceIn:
                    return Bounce.In(k);
                case Type.BounceOut:
                    return Bounce.Out(k);
                case Type.BounceInOut:
                    return Bounce.InOut(k);
            }
            
            return k; 
        }


        public static float Linear(float k)
        {
            return k;
        }

        public class Quadratic
        {
            public static float In(float k)
            {
                return k * k;
            }

            public static float Out(float k)
            {
                return k * (2f - k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k;
                return -0.5f * ((k -= 1f) * (k - 2f) - 1f);
            }
        };

        public class Cubic
        {
            public static float In(float k)
            {
                return k * k * k;
            }

            public static float Out(float k)
            {
                return 1f + ((k -= 1f) * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k;
                return 0.5f * ((k -= 2f) * k * k + 2f);
            }
        };

        public class Quartic
        {
            public static float In(float k)
            {
                return k * k * k * k;
            }

            public static float Out(float k)
            {
                return 1f - ((k -= 1f) * k * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k * k;
                return -0.5f * ((k -= 2f) * k * k * k - 2f);
            }
        };

        public class Quintic
        {
            public static float In(float k)
            {
                return k * k * k * k * k;
            }

            public static float Out(float k)
            {
                return 1f + ((k -= 1f) * k * k * k * k);
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * k * k * k * k * k;
                return 0.5f * ((k -= 2f) * k * k * k * k + 2f);
            }
        };

        public class Sinusoidal
        {
            public static float In(float k)
            {
                return 1f - Mathf.Cos(k * Mathf.PI / 2f);
            }

            public static float Out(float k)
            {
                return Mathf.Sin(k * Mathf.PI / 2f);
            }

            public static float InOut(float k)
            {
                return 0.5f * (1f - Mathf.Cos(Mathf.PI * k));
            }
        };

        public class Exponential
        {
            public static float In(float k)
            {
                return k == 0f ? 0f : Mathf.Pow(1024f, k - 1f);
            }

            public static float Out(float k)
            {
                return k == 1f ? 1f : 1f - Mathf.Pow(2f, -10f * k);
            }

            public static float InOut(float k)
            {
                if (k == 0f) return 0f;
                if (k == 1f) return 1f;
                if ((k *= 2f) < 1f) return 0.5f * Mathf.Pow(1024f, k - 1f);
                return 0.5f * (-Mathf.Pow(2f, -10f * (k - 1f)) + 2f);
            }
        };

        public class Circular
        {
            public static float In(float k)
            {
                return 1f - Mathf.Sqrt(1f - k * k);
            }

            public static float Out(float k)
            {
                return Mathf.Sqrt(1f - ((k -= 1f) * k));
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return -0.5f * (Mathf.Sqrt(1f - k * k) - 1);
                return 0.5f * (Mathf.Sqrt(1f - (k -= 2f) * k) + 1f);
            }
        };

        public class Elastic
        {
            public static float In(float k)
            {
                if (k == 0) return 0;
                if (k == 1) return 1;
                return -Mathf.Pow(2f, 10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f);
            }

            public static float Out(float k)
            {
                if (k == 0) return 0;
                if (k == 1) return 1;
                return Mathf.Pow(2f, -10f * k) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f) + 1f;
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return -0.5f * Mathf.Pow(2f, 10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f);
                return Mathf.Pow(2f, -10f * (k -= 1f)) * Mathf.Sin((k - 0.1f) * (2f * Mathf.PI) / 0.4f) * 0.5f + 1f;
            }
        };

        public class Back
        {
            static float s = 1.70158f;
            static float s2 = 2.5949095f;

            public static float In(float k)
            {
                return k * k * ((s + 1f) * k - s);
            }

            public static float Out(float k)
            {
                return (k -= 1f) * k * ((s + 1f) * k + s) + 1f;
            }

            public static float InOut(float k)
            {
                if ((k *= 2f) < 1f) return 0.5f * (k * k * ((s2 + 1f) * k - s2));
                return 0.5f * ((k -= 2f) * k * ((s2 + 1f) * k + s2) + 2f);
            }
        };

        public class Bounce
        {
            public static float In(float k)
            {
                return 1f - Out(1f - k);
            }

            public static float Out(float k)
            {
                if (k < (1f / 2.75f))
                {
                    return 7.5625f * k * k;
                }
                else if (k < (2f / 2.75f))
                {
                    return 7.5625f * (k -= (1.5f / 2.75f)) * k + 0.75f;
                }
                else if (k < (2.5f / 2.75f))
                {
                    return 7.5625f * (k -= (2.25f / 2.75f)) * k + 0.9375f;
                }
                else
                {
                    return 7.5625f * (k -= (2.625f / 2.75f)) * k + 0.984375f;
                }
            }

            public static float InOut(float k)
            {
                if (k < 0.5f) return In(k * 2f) * 0.5f;
                return Out(k * 2f - 1f) * 0.5f + 0.5f;
            }
        };
    }

}