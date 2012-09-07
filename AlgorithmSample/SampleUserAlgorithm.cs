using System;
using System.Collections;
using System.Text;

using Kuaff.Tractor.Plugins;

namespace Kuaff.Tractor.AlgorithmSample
{
    public class SampleUserAlgorithm : IUserAlgorithm
    {
        /// <summary>
        /// �㷨����
        /// </summary>
        public string Author
        {
            get {return "smallnest";}
        }
        /// <summary>
        /// �㷨���ߵ�email��ַ
        /// </summary>
        public string Email
        {
            get {return "smallnest@gmail.com";}
        }
        /// <summary>
        /// �㷨����
        /// </summary>
        public string Name
        {
            get {return "����ʾ�㷨";}
        }
        /// <summary>
        /// �㷨����
        /// </summary>
        public string Description
        {
            get { return "����һ���㷨�ӿڵļ�ʵ�֣�������ʾ���ʵ���㷨�ӿڡ�"; }
        }

        /// <summary>
        /// ���ȳ��Ƶ��㷨��
        /// </summary>
        /// <param name="who">��ǰ�û���˭��1Ϊ�ϼң�2Ϊ���ң�3Ϊ���ң�4Ϊ����</param>
        /// <param name="suit">��ǰ���ƵĻ�ɫ��1Ϊ���ģ�2Ϊ���ң�3Ϊ��Ƭ��4Ϊ÷��,5Ϊ����������</param>
        /// <param name="rank">��ǰ��,0Ϊ��2��1Ϊ��3,2Ϊ��4........11Ϊ��K��12Ϊ��A,53Ϊ����</param>
        /// <param name="master">��ǰ˭Ϊׯ��,1Ϊ�ϼң�2Ϊ���ң�3Ϊ���ң�4Ϊ����</param>
        /// <param name="sendCards">��ǰһ�ָ����Ѿ��������ƣ�sendCards[0]Ϊ�ϼң�sendCards[1]Ϊ���ң�sendCards[2]Ϊ���ң�sendCards[3]Ϊ����</param>
        /// <param name="myCards">���û����е���</param>
        /// <returns></returns>
        public ArrayList ShouldSendCards(int who, int suit, int rank, int master, string[] sendCards, string myCards)
        {
            ArrayList result = new ArrayList();
            string[] cards = myCards.Split(new char[] {','});
            if (cards.Length > 0)
            {
                result.Add(int.Parse(cards[0]));
            }
           
            return result;
        }

        /// <summary>
        /// ���Լ�������ʱ���㷨���Լ������׼ң�
        /// </summary>
        /// <param name="who">��ǰ�û���˭��1Ϊ�ϼң�2Ϊ���ң�3Ϊ���ң�4Ϊ����</param>
        /// <param name="suit">��ǰ���ƵĻ�ɫ��1Ϊ���ģ�2Ϊ���ң�3Ϊ��Ƭ��4Ϊ÷��,5Ϊ����������</param>
        /// <param name="rank">��ǰ��,0Ϊ��2��1Ϊ��3,2Ϊ��4........11Ϊ��K��12Ϊ��A,53Ϊ����</param>
        /// <param name="master">��ǰ˭Ϊׯ��,1Ϊ�ϼң�2Ϊ���ң�3Ϊ���ң�4Ϊ����</param>
        /// <param name="whoIsFirst">˭���ȳ����ƣ�1Ϊ�ϼң�2Ϊ���ң�3Ϊ���ң�4Ϊ����</param>
        /// <param name="sendCards">��ǰһ�ָ����Ѿ��������ƣ�sendCards[0]Ϊ�ϼң�sendCards[1]Ϊ���ң�sendCards[2]Ϊ���ң�sendCards[3]Ϊ����</param>
        /// <param name="currentSendCards">�׼��Լ��Լ����ϼҳ�����</param>
        /// <param name="myCards">���û����е���</param>
        /// <returns></returns>
        public ArrayList MustSendCards(int who, int suit, int rank, int master, int whoIsFirst, string[] sendCards, ArrayList[] currentSendCards, string myCards)
        {
            ArrayList result = new ArrayList();
            string[] cards = myCards.Split(new char[] { ',' });
            //�õ��׼ҳ��Ļ�ɫ����ĳ�ָ��ƻ����ڵ���
            
            if (currentSendCards[whoIsFirst-1].Count > 0)
            {
                //�õ��׼ҳ���һ����
                int oneCard = (int)currentSendCards[whoIsFirst - 1][0];
                bool isSuit = false;
                if (oneCard == 52 || oneCard == 53) //�����С�����ߴ�������������Ӧ������
                {
                    isSuit = true;
                }
                else if ((oneCard % 13) == rank) //�������,�����8�������������8����Ϊ����
                {
                    isSuit = true;
                }
                else
                {
                    if ((oneCard >= 0 && oneCard < 13) && (suit==1)) //��������Ǻ��Ķ�������ɫҲ�Ǻ���
                    {
                        isSuit = true;
                    }
                    else if ((oneCard >= 13 && oneCard < 26) && (suit == 2)) //��������Ǻ��Ҷ�������ɫҲ�Ǻ���
                    {
                        isSuit = true;
                    }
                    else if ((oneCard >= 26 && oneCard < 39) && (suit == 3)) //��������Ƿ�Ƭ��������ɫҲ�Ƿ�Ƭ
                    {
                        isSuit = true;
                    }
                    else if ((oneCard >= 39 && oneCard < 52) && (suit == 4)) //���������÷����������ɫҲ��÷��
                    {
                        isSuit = true;
                    }
                }
                int count = currentSendCards[whoIsFirst - 1].Count;

                if (isSuit) //����ǵ������������ķ�������
                {
                    //TODO:�ж��׼��Ƿ���˦��

                    //TODO:�����ж��׼��Ƿ���������

                    //TODO:�����ж��׼��Ƿ����

                    //TODO:�����׼�Ӧ�ó����ŵ���

                    //TODO:����ɾ������������İ취,�ٶ��׼�ֻ����һ����,����δ�������������
                    for (int i = 0; i < cards.Length; i++)
                    {
                        int number = int.Parse(cards[i]);
                        if ((number / 13) == (suit-1) && number < 52) //�д˻�ɫ����
                        {
                            result.Add(number);
                            break;
                        }
                        else if ((number == 52) || (number == 53)) //�д�С��
                        {
                            result.Add(number);
                            break;
                        }
                        else if ((number % 13) == rank) //�����������10,��10
                        {
                            result.Add(number);
                            break;
                        }
                    }

                    if (result.Count ==0) //�����������Ÿ���
                    {
                        result.Add(int.Parse(cards[0]));
                    }
                }
                else //�������Ӧ�ĸ���
                {
                    //TODO:�ж��׼��Ƿ���˦��

                    //TODO:�����ж��׼��Ƿ���������

                    //TODO:�����ж��׼��Ƿ����

                    //TODO:�����׼�Ӧ�ó����ŵ���

                    //TODO:����ɾ������������İ취,�ٶ��׼�ֻ����һ����,����δ�������������
                    int firstSuit = (oneCard / 13) + 1;

                    for (int i = 0; i < cards.Length; i++)
                    {
                        if (cards[i].Length == 0)
                        {
                            continue;
                        }
                        int number = int.Parse(cards[i]);
                        if ((number / 13) == (firstSuit - 1) && number < 52 && ((number % 13) != rank)) //�д˻�ɫ
                        {
                            result.Add(number);
                            break;
                        }
                       
                    }
                    if (result.Count == 0) //���û�д˻�ɫ���ƣ�����һ��
                    {
                        result.Add(int.Parse(cards[0]));
                    }
                }
                }

               

            return result;
        }
    }
}
