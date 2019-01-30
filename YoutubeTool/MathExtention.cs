using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.Extention
{
    /// <summary>�v�Z���s���g���֐��N���X</summary>
    public static class MathExtention
    {
        #region �W�v
        /// <summary>
        /// ���l�̍��v(����)
        /// </summary>
        /// <param name="src">�z��</param>
        public static Double Total(this IEnumerable<Double> src) => src.Sum();

        /// <summary>
        /// ���l�̍��v(����)
        /// </summary>
        /// <param name="src">�z��</param>
        public static Int32 Total(this IEnumerable<Int32> src) => src.Sum();

        /// <summary>
        /// �֐���p�������l�̍��v(����) ��
        /// </summary>
        /// <typeparam name="T">�z����\������^</typeparam>
        /// <param name="src">�z��</param>
        /// <param name="func">�֐�</param>
        public static Double Sigma<T>(this IEnumerable<T> src, Func<T, Double> func)
            => src.Select(arg => func(arg)).Sum();

        /// <summary>
        /// �֐���p�������l�̍��v(����) ��
        /// </summary>
        /// <typeparam name="T">�z����\������^</typeparam>
        /// <param name="src">�z��</param>
        /// <param name="func">�֐�</param>
        public static Int32 Sigma<T>(this IEnumerable<T> src, Func<T, Int32> func)
            => src.Select(arg => func(arg)).Sum();
        #endregion

        #region ����
        /// <summary>
        /// �z����̗v�f�̕��ϒl���v�Z
        /// </summary>
        /// <param name="src">�z��</param>
        /// <returns>���ϒl</returns>
        public static Double Average(this IEnumerable<Double> src)
        {
            return src.Sum() / src.Count();
        }

        /// <summary>
        /// �z����̗v�f�̕��ϒl���v�Z
        /// </summary>
        /// <param name="src">�z��</param>
        /// <returns>���ϒl</returns>
        public static Double AverageIf(this IEnumerable<Double> src,
                                            Func<Double, Boolean> selector)
        {
            var selectArray = src.Where(val => selector(val));  // �������ɂ�钊�o
            return selectArray.Sum() / selectArray.Count();     // ���ϒl�v�Z
        }
        #endregion

        #region �r�b�g�v�Z
        /// <summary>
        /// �r�b�g�t���O����
        /// </summary>
        /// <param name="value">���l</param>
        /// <param name="bit">�r�b�g(1�`)</param>
        public static Boolean BitFlag(this UInt32 value, Int32 bit)
            // �󂯎�������l�̎w�肳�ꂽ�r�b�g�������Ă��邩�m�F
            => (value & (1 << bit - 1)) != 0;
        
        /// <summary>
        /// �r�b�g�t���O����
        /// </summary>
        /// <param name="value">���l</param>
        /// <param name="bit">�r�b�g(1�`)</param>
        /// <remarks>�����t�ւ̑Ή��I�[�o�[���[�h</remarks>
        public static Boolean BitFlag(this Int32 value, Int32 bit)
        {   // �󂯎�������l�̎w�肳�ꂽ�r�b�g�������Ă��邩�m�F
            return (value & (1 << bit - 1)) != 0;
        }

        /// <summary>
        /// �r�b�g���J�E���g
        /// </summary>
        /// <param name="value">�J�E���g���鐔�l</param>
        /// <remarks>
        /// �}�C�i�X�l������Ȃ��悤��
        /// �������ł̒l��ݒ�
        /// </remarks>
        public static Int32 BitCount(this UInt32 value)
        {
            var bits = (Int32)value;
            bits = (bits & 0x55555555) + (bits >> 1 & 0x55555555);
            bits = (bits & 0x33333333) + (bits >> 2 & 0x33333333);
            bits = (bits & 0x0f0f0f0f) + (bits >> 4 & 0x0f0f0f0f);
            bits = (bits & 0x00ff00ff) + (bits >> 8 & 0x00ff00ff);
            return (bits & 0x0000ffff) + (bits >> 16 & 0x0000ffff);
        }

        /// <summary>���l��Bit(0,1)�̔z��ɕϊ�</summary>
        public static UInt32[] ToBitArray(this UInt32 value)
        {
            return Array.ConvertAll(Convert.ToString(value, 2).ToCharArray(), c => ((UInt32)c) - 48);
        }
        #endregion

        #region ���������킹�֐� floor��ceiling�����o�I�ɓ���܂Ȃ��׍쐬
        /// <summary>
        /// �w�肵�����x�̐��l�Ɏl�̌ܓ�����
        /// </summary>
        /// <param name="value">���̒l</param>
        /// <param name="digits">�L������</param>
        /// <remarks>
        /// Math�N���X��Floor��Ceillng�̋�����
        /// ����e���񂾎l�̌ܓ��ł͂Ȃ��̂ō쐬�B
        /// (�ȉ�Down, Up�����l)
        /// </remarks>
        public static Double Round(this Double value, Int32 digits)
        {
            Double coef = Math.Pow(10, digits); //�ϊ��p�W��

            return value > 0 ? Math.Floor((value * coef) + 0.5) / coef 
                             : Math.Ceiling((value * coef) - 0.5) / coef;
        }

        /// <summary>
        /// �w�肵�����x�̐��l�ɐ؎̂Ă�
        /// </summary>
        /// <param name="value">���̒l</param>
        /// <param name="digits">�L������</param>
        public static Double RoundDown(this Double value, Int32 digits)
        {
            Double coef = Math.Pow(10, digits);

            return value > 0 ? Math.Floor(value * coef) / coef 
                             : Math.Ceiling(value * coef) / coef;
        }

        /// <summary>
        /// �w�肵�����x�̐��l�ɐ؏グ��
        /// </summary>
        /// <param name="value">���̒l</param>
        /// <param name="digits">�L������</param>
        public static Double RoundUp(this Double value, Int32 digits)
        {
            Double coef = Math.Pow(10, digits);

            return value > 0 ? Math.Ceiling(value * coef) / coef
                             : Math.Floor(value * coef) / coef;
        }

        /// <summary>
        /// �����ɂ���ׂ̎l�̌ܓ�
        /// </summary>
        /// <param name="value">���̒l</param>
        /// <returns>�����l</returns>
        public static Int32 Round(this Double value)
        {
            Double coef = Math.Pow(10, 0); //�ϊ��p�W��

            return value > 0 ? (Int32)(Math.Floor((value * coef) + 0.5) / coef)
                             : (Int32)(Math.Ceiling((value * coef) - 0.5) / coef);
        }

        /// <summary>
        /// �����ɂ���ׂ̒[���؎̂�
        /// </summary>
        /// <param name="value">���̒l</param>
        /// <returns>�����l</returns>
        public static Int32 RoundDown(this Double value)
        {
            Double coef = Math.Pow(10, 0);

            return value > 0 ? (Int32)(Math.Floor(value * coef) / coef)
                             : (Int32)(Math.Ceiling(value * coef) / coef);
        }

        /// <summary>
        /// �����ɂ���ׂ̒[���؏グ
        /// </summary>
        /// <param name="value">���̒l</param>
        /// <returns>�����l</returns>
        public static Double RoundUp(this Double value)
        {
            Double coef = Math.Pow(10, 0);

            return value > 0 ? (Int32)(Math.Ceiling(value * coef) / coef)
                             : (Int32)(Math.Floor(value * coef) / coef);
        }
        #endregion

        #region Math�N���X���g���֐��Ƀ��b�v
        /// <summary>
        /// �p��̌v�Z
        /// </summary>
        /// <param name="value">��</param>
        /// <param name="exponent">�w��</param>
        public static Double Pow(this Double value, UInt32 exponent)
            => Math.Pow(value, exponent);

        /// <summary>
        /// �p��̌v�Z
        /// </summary>
        /// <param name="value">��</param>
        /// <param name="exponent">�w��</param>
        public static Int32 Pow(this Int32 value, UInt32 exponent)
            => (Int32)Math.Pow(value, exponent);

        /// <summary>��Βl�̎擾</summary>
        /// <param name="value">���̒l</param>
        public static Double Abs(this Double value) => Math.Abs(value);

        /// <summary>��Βl�̎擾</summary>
        /// <param name="value">���̒l</param>
        public static Int32 Abs(this Int32 value) => Math.Abs(value);
        #endregion

        #region �����_�ȉ����擾
        /// <summary>
        /// �����_�ȉ��̒l���擾����
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Double GetDecimal(this Double value)
        {
            return value % 1;
        }
        #endregion


        /// <summary>���l�ɕϊ��ł���H</summary>
        public static Boolean IsNumeric(this ValueType src)
        {
            // ���l�^�̂����ꂩ�Ƀq�b�g����H
            return (src is Byte || src is Int16 || src is Int32 || src is Int64 ||
                    src is SByte || src is UInt16 || src is UInt32 || src is UInt64 ||
                    src is Decimal || src is Double || src is Single);
        }
    }
}