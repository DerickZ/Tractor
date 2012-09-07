using System;
using System.Collections;
using System.Text;

namespace Kuaff.Tractor.Plugins
{
    public interface IUserAlgorithm
    {
        /// <summary>
        /// �㷨����
        /// </summary>
        string Author
        {
            get;
        }
        /// <summary>
        /// �㷨���ߵ�email��ַ
        /// </summary>
        string Email
        {
            get;
        }
        /// <summary>
        /// �㷨����
        /// </summary>
        string Name
        {
            get;
        }
        /// <summary>
        /// �㷨����
        /// </summary>
        string Description
        {
            get;
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
        ArrayList ShouldSendCards(int who, int suit, int rank, int master, string[] sendCards, string myCards);
        
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
        ArrayList MustSendCards(int who, int suit, int rank, int master, int whoIsFirst, string[] sendCards, ArrayList[] currentSendCards, string myCards);
    }
}
