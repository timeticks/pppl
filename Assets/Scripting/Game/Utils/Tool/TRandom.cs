/*
 * Copyright (c) 2008 Microsoft::Tsorgy.Utils, Reserved.
 * 
 * Filename:    @(#)Random.cs
 * Create by:   TsOrgY
 * Email:       tsorgy@gmail.com
 * Date:        2008/12/27 15:01:40
 * Description: 一种能够产生满足某些随机性统计要求的数字序列的设备.
 *              
 */
using System;
namespace Tsorgy {
    /// <summary>
    /// 表示伪随机数生成器，一种能够产生满足某些随机性统计要求的数字序列的设备.
    /// </summary>
    [Serializable]
    public class TRandom {
        private int inext;
        private int inextp;
        private int[] SeedArray;
        /// <summary>
        /// 使用与时间相关的默认种子值，初始化 Random 类的新实例.
        /// </summary>
        public TRandom()
            : this(Environment.TickCount) {
        }
        /// <summary>
        /// 使用指定的种子值初始化 System.Random 类的新实例.
        /// </summary>
        /// <param name="Seed">用来计算伪随机数序列起始值的数字。如果指定的是负数，则使用其绝对值。</param>
        /// <exception cref="System.OverflowException">Seed 为 System.Int32.MinValue，在计算其绝对值时会导致溢出。</exception>
        public TRandom(int Seed) {
            this.SeedArray = new int[0x38];
            int num2 = 0x9a4ec86 - Math.Abs(Seed);
            this.SeedArray[0x37] = num2;
            int num3 = 1;
            for (int i = 1; i < 0x37; i++) {
                int index = (0x15 * i) % 0x37;
                this.SeedArray[index] = num3;
                num3 = num2 - num3;
                if (num3 < 0)
                {
                    num3 += 0x7fffffff;
                }
                num2 = this.SeedArray[index];
            }
            for (int j = 1; j < 5; j++) {
                for (int k = 1; k < 0x38; k++) {
                    this.SeedArray[k] -= this.SeedArray[1 + ((k + 30) % 0x37)];
                    if (this.SeedArray[k] < 0)
                    {
                        this.SeedArray[k] += 0x7fffffff;
                    }
                }
            }
            this.inext = 0;
            this.inextp = 0x15;
            Seed = 1;
        }
        private double GetSampleForLargeRange() {
            int num = this.InternalSample();
            if ((((this.InternalSample() % 2) == 0) ? 1 : 0) != 0) {
                num = -num;
            }
            double num2 = num;
           // num2 += 2147483646.0;
            return (num2 / 2147483647);//2147483647    4294967293
        }
        private int InternalSample() {
            int inext = this.inext;
            int inextp = this.inextp;
            if (++inext >= 0x38) {
                inext = 1;
            }
            if (++inextp >= 0x38) {
                inextp = 1;
            }
            int num = this.SeedArray[inext] - this.SeedArray[inextp];
            if (num < 0) {
                num += 0x7fffffff;
            }
            this.SeedArray[inext] = num;
            this.inext = inext;
            this.inextp = inextp;
            return num;
        }
        /// <summary>
        /// 返回非负随机数.
        /// </summary>
        /// <returns>大于或等于零且小于 System.Int32.MaxValue 的 32 位带符号整数。</returns>
        public virtual int Next() {
            return this.InternalSample();
        }
        /// <summary>
        /// 返回一个小于所指定最大值的非负随机数.
        /// </summary>
        /// <param name="maxValue">要生成的随机数的上界（随机数不能取该上界值）。maxValue 必须大于或等于零。</param>
        /// <returns>大于或等于零且小于 maxValue 的 32 位带符号整数，即：返回的值范围包括零但不包括 maxValue。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">maxValue 小于零。</exception>
        public virtual int Next(int maxValue) {
            if (maxValue < 0) {
                throw new ArgumentOutOfRangeException("maxValue", string.Format("'{0}' must be greater than zero.", maxValue));
            }
            return (int) (this.Sample() * maxValue);
        }
        /// <summary>
        /// 返回一个指定范围内的随机数.
        /// </summary>
        /// <param name="minValue">返回的随机数的下界（随机数可取该下界值）。</param>
        /// <param name="maxValue">返回的随机数的上界（随机数不能取该上界值）。maxValue 必须大于或等于 minValue。</param>
        /// <returns>一个大于或等于 minValue 且小于 maxValue 的 32 位带符号整数，即：返回的值范围包括 minValue 但不包括 maxValue。如果minValue 等于 maxValue，则返回 minValue。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">minValue 大于 maxValue。</exception>
        public virtual int Next(int minValue, int maxValue) {
            if (minValue > maxValue) {
                throw new ArgumentOutOfRangeException("minValue", string.Format("'{0}' cannot be greater than {1}.", minValue, maxValue));
            }
            long num = maxValue - minValue;
            if (num <= 0x7fffffffL) {
                return (((int) (this.Sample() * num)) + minValue);
            }
            return (((int) ((long) (this.GetSampleForLargeRange() * num))) + minValue);
        }
        /// <summary>
        /// 用随机数填充指定字节数组的元素.
        /// </summary>
        /// <param name="buffer">包含随机数的字节数组。</param>
        /// <exception cref="System.ArgumentNullException">buffer 为 null。</exception>
        public virtual void NextBytes(byte[] buffer) {
            if (buffer == null) {
                throw new ArgumentNullException("buffer");
            }
            for (int i = 0; i < buffer.Length; i++) {
                buffer[i] = (byte) (this.InternalSample() % 0x100);
            }
        }
        /// <summary>
        /// 返回一个介于 0.0 和 1.0 之间的随机数.
        /// </summary>
        /// <returns>大于或等于 0.0 而小于 1.0 的双精度浮点数字。</returns>
        public virtual double NextDouble() {
            return this.Sample();
        }
        /// <summary>
        /// 返回一个介于 0.0 和 1.0 之间的随机数.
        /// </summary>
        /// <returns>大于或等于 0.0 而小于 1.0 的双精度浮点数字。</returns>
        protected virtual double Sample() {
            return (this.InternalSample() * 4.6566128752457969E-10);
        }
    }
}