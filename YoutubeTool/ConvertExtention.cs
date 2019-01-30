using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Project.Extention
{
    /// <summary>�l�̌^�ϊ��Ɋւ���g���֐��N���X</summary>
    public static class ConvertExtention
    {
        /// <summary>
        /// �l�̕ϊ�
        /// </summary>
        /// <typeparam name="TOutput">�ϊ���̌^</typeparam>
        /// <param name="value">�ϊ����Ƃ̒l</param>
        public static TOutput To<TOutput>(this Object value)
        {
            var result = default(TOutput);
            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(TOutput));
                if (converter != null)
                {
                    result = (TOutput)converter.ConvertTo(value, typeof(TOutput));
                }
            }
            catch (Exception) { result = default(TOutput); }
            return result;
        }

        /// <summary>
        /// �^���w�肵�Ă̕ϊ�
        /// </summary>
        /// <param name="value">�ϊ����Ƃ̒l</param>
        /// <param name="type">�ϊ���̌^</param>
        /// <remarks>�߂�l�̃L���X�g���K�v</remarks>
        public static Object To(this Object value, Type type)
        {
            Object result = type.IsValueType ? Activator.CreateInstance(type) : null;
            // �^�R���o�[�^�쐬
            TypeConverter converter = TypeDescriptor.GetConverter(type);

            // �^�R���o�[�^���쐬�ł��Ă���Ό^��ϊ�����
            if (converter != null)
            {
                result = converter.ConvertTo(value, type);
            }
            return result;
        }

        /// <summary>
        /// �l�̕ϊ�(�ꎟ�z��)
        /// </summary>
        /// <typeparam name="TInput">�ϊ����Ƃ̌^</typeparam>
        /// <typeparam name="TOutput">�ϊ���̌^</typeparam>
        /// <param name="values">�ϊ����Ƃ̒l</param>
        public static TOutput[] ConvertAll<TInput, TOutput>(this IEnumerable<TInput> values)
        {
            if (values == null) { return null; }
            return Array.ConvertAll(values.ToArray(), val => val.To<TOutput>());
        }

        /// <summary>
        /// �l�̕ϊ�(�񎟔z��)
        /// </summary>
        /// <typeparam name="TInput">�ϊ����Ƃ̌^</typeparam>
        /// <typeparam name="TOutput">�ϊ���̌^</typeparam>
        /// <param name="values">�ϊ����Ƃ̒l</param>
        public static TOutput[][] ConvertAll<TInput, TOutput>(this IEnumerable<IEnumerable<TInput>> values)
        {
            if (values == null) { return null; }

            var result = new TOutput[values.Count()][];
            Int32 count = 0;
            foreach (var val in values)
            {
                result[count] = val?.ConvertAll<TInput, TOutput>();
                count++;
            }
            return result;
        }

        /// <summary>
        /// �z���Object�^�֕ϊ�����
        /// </summary>
        /// <typeparam name="T">�ϊ����Ƃ̌^</typeparam>
        /// <param name="src">�ϊ����s���z��</param>
        /// <returns>Object�^�ɕϊ����ꂽ�z��</returns>
        public static Object ToObject<T>(this T[] src)
        {
            return src;
        }
    }
}


