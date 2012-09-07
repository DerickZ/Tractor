using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using Kuaff.CardResouces;

namespace Kuaff.Tractor
{
    /// <summary>
    /// �ṩ�����Ĺ���
    /// </summary>
    class TractorRules
    {

        //�ж��ҳ������Ƿ�Ϸ�
        internal static bool IsInvalid(MainForm mainForm, ArrayList[] currentSendedCards, int who)
        {
            
            CurrentPoker[] cp = new CurrentPoker[4];
            int suit = mainForm.currentState.Suit;
            int first = mainForm.firstSend;

            int rank = mainForm.currentRank;

            cp[who-1] = new CurrentPoker();
            cp[who - 1].Suit = suit;
            cp[who - 1].Rank = rank;

            ArrayList list = new ArrayList();
            CurrentPoker tmpCP = new CurrentPoker();
            tmpCP.Suit = suit;
            tmpCP.Rank = rank;

            for (int i = 0; i < mainForm.myCardIsReady.Count; i++)
            {
                if ((bool)mainForm.myCardIsReady[i])
                {
                    cp[who - 1].AddCard((int)mainForm.myCardsNumber[i]);
                    list.Add((int)mainForm.myCardsNumber[i]);
                }
                else
                {
                    tmpCP.AddCard((int)mainForm.myCardsNumber[i]);
                }
            }
            int[] users = CommonMethods.OtherUsers(who);

            cp[users[0] - 1] = CommonMethods.parse(mainForm.currentSendCards[users[0] - 1], suit, rank);
            cp[users[1] - 1] = CommonMethods.parse(mainForm.currentSendCards[users[1] - 1], suit, rank);
            cp[users[2] - 1] = CommonMethods.parse(mainForm.currentSendCards[users[2] - 1], suit, rank);
            cp[0].Sort();
            cp[1].Sort();
            cp[2].Sort();
            cp[3].Sort();

            

            //����ҳ���
            if (first == who)
            {
                if (cp[who -1].Count ==0)
                {
                    return false;
                }

                if (cp[who-1].IsMixed())
                {
                    return false;
                }

                return true;
            }
            else
            {
                if (list.Count != currentSendedCards[first - 1].Count)
                {
                    return false;
                }


                //�õ���һ���һ���Ļ�ɫ
                int previousSuit = CommonMethods.GetSuit((int)currentSendedCards[first - 1][0], suit, rank);
               
                //0.������ǻ�ϵģ����ж��������Ƿ�ʣ���Ļ�ɫ�����ʣ,false;�����ʣ;true
                if (cp[who-1].IsMixed())
                {
                    if (tmpCP.HasSomeCards(previousSuit))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }


                //������Ļ�ɫ��һ��
                int mysuit = CommonMethods.GetSuit((int)list[0], suit, rank);


                //���ȷʵ��ɫ��һ��
                if (mysuit != previousSuit) 
                {
                    //����ȷʵû�д˻�ɫ
                    if (tmpCP.HasSomeCards(previousSuit))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }


                //3.����������ԣ���ҲӦ�ó���
                int firstPairs = cp[first - 1].GetPairs().Count;
                int mypairs = cp[who - 1].GetPairs().Count;
                int myCurrentPairs = mainForm.currentPokers[who - 1].GetPairs(previousSuit).Count;


                //2.������˳���������������У�ҲӦ�ó�������
                if (cp[first-1].HasTractors())
                {
                    if ((!cp[who - 1].HasTractors()) && (mainForm.currentPokers[who-1].GetTractor(previousSuit) > -1))
                    {
                        return false;
                    }
                    else if ((mypairs == 1) && (myCurrentPairs> 1)) //���˵����ԣ�����ʵ�ʶ���1����
                    {
                        return false;
                    }
                    else if ((mypairs == 0) && (myCurrentPairs > 0)) //û���ԣ���ʵ���ж�
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

                

                if (firstPairs > 0)
                {
                    if ((myCurrentPairs >= firstPairs) && (mypairs < firstPairs))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

            }
            return true;

        }

        internal static bool IsInvalid(MainForm mainForm, ArrayList[] currentSendedCards,ArrayList currentSendedCard, int who)
        {

            CurrentPoker[] cp = new CurrentPoker[4];
            int suit = mainForm.currentState.Suit;
            int first = mainForm.firstSend;

            int rank = mainForm.currentRank;

            cp[who - 1] = new CurrentPoker();
            cp[who - 1].Suit = suit;
            cp[who - 1].Rank = rank;

            ArrayList list = new ArrayList();
            CurrentPoker tmpCP = new CurrentPoker();
            tmpCP.Suit = suit;
            tmpCP.Rank = rank;

            for (int i = 0; i < currentSendedCard.Count; i++)
            {

                cp[who - 1].AddCard((int)currentSendedCard[i]);
                list.Add((int)currentSendedCard[i]);
               
            }
            int[] users = CommonMethods.OtherUsers(who);

            cp[users[0] - 1] = CommonMethods.parse(mainForm.currentSendCards[users[0] - 1], suit, rank);
            cp[users[1] - 1] = CommonMethods.parse(mainForm.currentSendCards[users[1] - 1], suit, rank);
            cp[users[2] - 1] = CommonMethods.parse(mainForm.currentSendCards[users[2] - 1], suit, rank);
            cp[0].Sort();
            cp[1].Sort();
            cp[2].Sort();
            cp[3].Sort();



            //����ҳ���
            if (first == who)
            {
                if (cp[who-1].Count == 0)
                {
                    return false;
                }

                if (cp[who - 1].IsMixed())
                {
                    return false;
                }

                return true;
            }
            else
            {
                if (list.Count != currentSendedCards[first - 1].Count)
                {
                    return false;
                }


                //�õ���һ���һ���Ļ�ɫ
                int previousSuit = CommonMethods.GetSuit((int)currentSendedCards[first - 1][0], suit, rank);

                //0.������ǻ�ϵģ����ж��������Ƿ�ʣ���Ļ�ɫ�����ʣ,false;�����ʣ;true
                if (cp[who - 1].IsMixed())
                {
                    if (tmpCP.HasSomeCards(previousSuit))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }


                //������Ļ�ɫ��һ��
                int mysuit = CommonMethods.GetSuit((int)list[0], suit, rank);


                //���ȷʵ��ɫ��һ��
                if (mysuit != previousSuit)
                {
                    //����ȷʵû�д˻�ɫ
                    if (tmpCP.HasSomeCards(previousSuit))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }


                //3.����������ԣ���ҲӦ�ó���
                int firstPairs = cp[first - 1].GetPairs().Count;
                int mypairs = cp[who - 1].GetPairs().Count;
                int myCurrentPairs = mainForm.currentPokers[who - 1].GetPairs(previousSuit).Count;


                //2.������˳���������������У�ҲӦ�ó�������
                if (cp[first - 1].HasTractors())
                {
                    if ((!cp[who - 1].HasTractors()) && (mainForm.currentPokers[who - 1].GetTractor(previousSuit) > -1))
                    {
                        return false;
                    }
                    else if ((mypairs == 1) && (myCurrentPairs > 1)) //���˵����ԣ�����ʵ�ʶ���1����
                    {
                        return false;
                    }
                    else if ((mypairs == 0) && (myCurrentPairs > 0)) //û���ԣ���ʵ���ж�
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }



                if (firstPairs > 0)
                {
                    if ((myCurrentPairs >= firstPairs) && (mypairs < firstPairs))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }

            }
            return true;

        }


        //���ݵ÷��ж�Ӧ��������
        internal static void GetNextRank(MainForm mainForm, bool success)
        {
            
            int user = mainForm.currentState.Master; //�򱾴�ʱ����
            int rank = 0;

            int number = 0;
            if (success)
            {
                if (mainForm.Scores == 0)  //���
                {
                    number += 3;
                }
                else if ((mainForm.Scores >= 0) && (mainForm.Scores < 40)) //С��
                {
                    number += 2;
                }
                else
                {
                    number++;
                }
            }
            else
            {
                number = (mainForm.Scores - 80) / 40;
            }


            
            string mustRank = "," + mainForm.gameConfig.MustRank + ",";

            if ((user == 1) || (user == 2))
            {
                rank = mainForm.currentState.OurCurrentRank;
                int oldRank = rank;

                if (rank == 53)
                {
                    rank = 13;
                }
                rank += number;

                //�ж��Ƿ�ش�
                if (oldRank < 3 && rank > 3)
                {
                    if (mustRank.IndexOf(",3,")>=0)
                    {
                        rank = 3;
                    }
                }
                else if (oldRank < 8 && rank > 8)
                {
                    if (mustRank.IndexOf(",8,") >= 0)
                    {
                        rank = 8;
                    }
                }
                else if (oldRank < 9 && rank > 9)
                {
                    if (mustRank.IndexOf(",9,") >= 0)
                    {
                        rank = 9;
                    }
                }
                else if (oldRank < 10 && rank > 10)
                {
                    if (mustRank.IndexOf(",10,") >= 0)
                    {
                        rank = 10;
                    }
                }
                else if (oldRank < 11 && rank > 11)
                {
                    if (mustRank.IndexOf(",11,") >= 0)
                    {
                        rank = 11;
                    }
                }
                else if (oldRank < 12 && rank > 12)
                {
                    if (mustRank.IndexOf(",12,") >= 0)
                    {
                        rank = 12;
                    }
                }
                else if (oldRank < 13 && rank > 13)
                {
                    if (mustRank.IndexOf(",13,") >= 0)
                    {
                        rank = 13;
                    }
                }


                if (rank > 13)
                {
                    if ((user == 1) || (user == 2))
                    {
                        mainForm.currentState.OurTotalRound++;
                    }
                    else
                    {
                        mainForm.currentState.OpposedTotalRound++;
                    }
                    rank -= 14;
                }
                else if (rank == 13)
                {
                    rank = 53;
                }

              
                mainForm.currentState.OurCurrentRank = rank;
                mainForm.currentRank = rank;
            }
            else if ((user == 3) || (user == 4))
            {
                rank = mainForm.currentState.OpposedCurrentRank;
                int oldRank = rank;

                if (rank == 53)
                {
                    rank = 13;
                }
                rank += number;

                //�ж��Ƿ�ش�
                if (oldRank < 3 && rank > 3)
                {
                    if (mustRank.IndexOf(",3,") >= 0)
                    {
                        rank = 3;
                    }
                }
                else if (oldRank < 8 && rank > 8)
                {
                    if (mustRank.IndexOf(",8,") >= 0)
                    {
                        rank = 8;
                    }
                }
                else if (oldRank < 9 && rank > 9)
                {
                    if (mustRank.IndexOf(",9,") >= 0)
                    {
                        rank = 9;
                    }
                }
                else if (oldRank < 10 && rank > 10)
                {
                    if (mustRank.IndexOf(",10,") >= 0)
                    {
                        rank = 10;
                    }
                }
                else if (oldRank < 11 && rank > 11)
                {
                    if (mustRank.IndexOf(",11,") >= 0)
                    {
                        rank = 11;
                    }
                }
                else if (oldRank < 12 && rank > 12)
                {
                    if (mustRank.IndexOf(",12,") >= 0)
                    {
                        rank = 12;
                    }
                }
                else if (oldRank < 13 && rank > 13)
                {
                    if (mustRank.IndexOf(",13,") >= 0)
                    {
                        rank = 13;
                    }
                }

                if (rank > 13)
                {
                    rank -= 13;
                }
                else if (rank == 13)
                {
                    rank = 53;
                }

                
                mainForm.currentState.OpposedCurrentRank = rank;
                mainForm.currentRank = rank;
            }

        }


        //���һ���Ƿ�ס�˵�
        internal static bool IsMasterOK(MainForm mainForm, int who)
        {
            bool success = false;

            if (mainForm.currentState.Master == 1)
            {
                if ((who == 1) || (who == 2))
                {
                    success = true;
                }
            }
            else if (mainForm.currentState.Master == 2)
            {
                if ((who == 1) || (who == 2))
                {
                    success = true;
                    
                }
               
            }
            else if (mainForm.currentState.Master == 3)
            {
                if ((who == 3) || (who == 4))
                {
                    success = true;
                }
                
            }
            else if (mainForm.currentState.Master == 4)
            {
                if ((who == 3) || (who == 4))
                {
                    success = true;
                }
            }

            return success;
        }

        //�Ƿ�ɹ�
        internal static int CalculateNextMaster(MainForm mainForm,bool success)
        {
            int master = mainForm.currentState.Master;

            if (mainForm.currentState.Master == 1)
            {
                if (success)
                {
                    master = 2;
                }
                else
                {
                    master = 4;
                }
            }
            else if (mainForm.currentState.Master == 2)
            {
                if (success)
                {
                    master = 1;
                }
                else
                {
                    master = 3;
                }
            }
            else if (mainForm.currentState.Master == 3)
            {
                if (success)
                {
                    master = 4;
                }
                else
                {
                    master = 1;
                }
            }
            else if (mainForm.currentState.Master == 4)
            {
                if (success)
                {
                    master = 3;
                }
                else
                {
                    master = 2;
                }
            }

            return master;
        }

       
        internal static void GetNextMasterUser(MainForm mainForm)
        {
            

            //���һ��˭Ӯ��
            int who = GetNextOrder(mainForm);
            //ȷ���Ƿ�ס��
            bool lastMasterOk = IsMasterOK(mainForm,who);

            CurrentPoker CP = new CurrentPoker();
            CP.Suit = mainForm.currentState.Suit;
            CP.Rank = mainForm.currentRank;
            CP = CommonMethods.parse(mainForm.currentSendCards[who - 1],CP.Suit,CP.Rank);

           
            if (!lastMasterOk)
            {
                CalculateScore(mainForm);
                int howmany = 2;

                if (CP.HasTractors()) //TODO:�����ǳ�������
                {
                    howmany = 8;
                }
                else if (CP.GetPairs().Count > 0)
                {
                    howmany = 4;
                }
                else
                {
                    howmany = 2;
                }

                //�����ܵ÷�
                Calculate8CardsScore(mainForm, howmany);
            }


            //�Ѿ����㱾�ε��ܵ÷�

            //�Ƿ�ɹ�����,С��80��,�ɹ�����
            bool success = mainForm.Scores < 80;
            int oldMaster = mainForm.currentState.Master;

            int master = CalculateNextMaster(mainForm, success);

            mainForm.currentState.Master = master;

            GetNextRank(mainForm, success);

            //J����,Q����
            if (mainForm.gameConfig.JToBottom && (CP.Rank == 9) && (!success))
            {
                if (mainForm.currentSendCards[who - 1].Contains(9) || mainForm.currentSendCards[who - 1].Contains(22) || mainForm.currentSendCards[who - 1].Contains(35) || mainForm.currentSendCards[who - 1].Contains(48))
                {
                    if ((oldMaster == 1) || (oldMaster == 2))
                    {
                        mainForm.currentState.OurCurrentRank = 0;
                    }
                    if ((oldMaster == 3) || (oldMaster == 4))
                    {
                        mainForm.currentState.OpposedCurrentRank = 0;
                    }
                }
            }
            if (mainForm.gameConfig.QToHalf && (CP.Rank == 10) && (!success))
            {
                if (mainForm.currentSendCards[who - 1].Contains(10) || mainForm.currentSendCards[who - 1].Contains(23) || mainForm.currentSendCards[who - 1].Contains(36) || mainForm.currentSendCards[who - 1].Contains(49))
                {
                    if ((oldMaster == 1) || (oldMaster == 2))
                    {
                        mainForm.currentState.OurCurrentRank = 4;
                    }
                    if ((oldMaster == 3) || (oldMaster == 4))
                    {
                        mainForm.currentState.OpposedCurrentRank = 4;
                    }
                }
            }

            if (mainForm.gameConfig.AToJ && (CP.Rank == 12) && (!success))
            {
                if (mainForm.currentSendCards[who - 1].Contains(12) || mainForm.currentSendCards[who - 1].Contains(25) || mainForm.currentSendCards[who - 1].Contains(38) || mainForm.currentSendCards[who - 1].Contains(51))
                {
                    if ((oldMaster == 1) || (oldMaster == 2))
                    {
                        mainForm.currentState.OurCurrentRank = 9;
                    }
                    if ((oldMaster == 3) || (oldMaster == 4))
                    {
                        mainForm.currentState.OpposedCurrentRank = 9;
                    }
                }
            }
        }

       
        //ȷ����һ�θ�˭����
        internal static int GetNextOrder(MainForm mainForm)
        {
            CurrentPoker[] cp = new CurrentPoker[4];
            int suit = mainForm.currentState.Suit;
            int rank = mainForm.currentRank;
            cp[0] = CommonMethods.parse(mainForm.currentSendCards[0], suit, rank);
            cp[1] = CommonMethods.parse(mainForm.currentSendCards[1], suit, rank);
            cp[2] = CommonMethods.parse(mainForm.currentSendCards[2], suit, rank);
            cp[3] = CommonMethods.parse(mainForm.currentSendCards[3], suit, rank);
            cp[0].Sort();
            cp[1].Sort();
            cp[2].Sort();
            cp[3].Sort();



            int count = mainForm.currentSendCards[0].Count;
            

            int order = mainForm.firstSend;

            int firstSuit = CommonMethods.GetSuit((int)mainForm.currentSendCards[order-1][0],suit,rank);



            int[] users = CommonMethods.OtherUsers(order);

            //����ǻ���ƣ�˦�ƻ��߶���ԣ�,�����׼�order
            if ((cp[order - 1].HasTractors()) && (cp[order - 1].Count > 4)) //�ж��е�����
            {
                int orderMax = cp[order - 1].GetTractor();
                if (cp[users[0] - 1].HasTractors() && (!cp[users[0] - 1].IsMixed()))
                {
                    int tmpMax = cp[users[0] - 1].GetTractor();
                    if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                    {
                        order = users[0];
                        orderMax = tmpMax;
                    }
                }
                if (cp[users[1] - 1].HasTractors() && (!cp[users[1] - 1].IsMixed()))
                {
                    int tmpMax = cp[users[1] - 1].GetTractor();
                    if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                    {
                        order = users[1];
                        orderMax = tmpMax;
                    }
                }
                if (cp[users[2] - 1].HasTractors() && (!cp[users[2] - 1].IsMixed()))
                {
                    int tmpMax = cp[users[2] - 1].GetTractor();
                    if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                    {
                        order = users[2];
                        orderMax = tmpMax;
                    }
                }
            }
            if ((cp[order -1].GetPairs().Count*2 < count) && (cp[order -1].GetPairs().Count>0)) //�ж��е�����
            {
                //����е�����
                int orderMax = (int)cp[order - 1].GetPairs()[0];
                if (cp[users[0] - 1].GetPairs().Count > 0 && (!cp[users[0] - 1].IsMixed()))
                {
                    int tmpMax = (int)cp[users[0] - 1].GetPairs()[0];
                    if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                    {
                        order = users[0];
                        orderMax = tmpMax;
                    }
                }
                if (cp[users[1] - 1].GetPairs().Count > 0 && (!cp[users[1] - 1].IsMixed()))
                {
                    int tmpMax = (int)cp[users[1] - 1].GetPairs()[0];
                    if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                    {
                        order = users[1];
                        orderMax = tmpMax;
                    }
                }
                if (cp[users[2] - 1].GetPairs().Count > 0 && (!cp[users[2] - 1].IsMixed()))
                {
                    int tmpMax = (int)cp[users[2] - 1].GetPairs()[0];
                    if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                    {
                        order = users[2];
                        orderMax = tmpMax;
                    }
                }

            }
            else if ((count> 1) && (cp[order -1].GetPairs().Count == 0)) //˦���������
            {
                int orderMax = (int)mainForm.currentSendCards[order - 1][0];
                int tmpMax = (int)mainForm.currentSendCards[users[0] - 1][0]; 
                if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                {
                    order = users[0];
                    orderMax = tmpMax;
                }

                tmpMax = (int)mainForm.currentSendCards[users[1] - 1][0]; 
                if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                {
                    order = users[1];
                    orderMax = tmpMax;
                }
                tmpMax = (int)mainForm.currentSendCards[users[2] - 1][0]; 
                if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                {
                    order = users[2];
                    orderMax = tmpMax;
                }
            }
            
            else if (cp[order - 1].HasTractors())
            {
                //�����������
                int orderMax = cp[order - 1].GetTractor();
                if (cp[users[0] - 1].HasTractors())
                {
                    int tmpMax = cp[users[0] - 1].GetTractor();
                    if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                    {
                        order = users[0];
                        orderMax = tmpMax;
                    }
                }
                if (cp[users[1] - 1].HasTractors())
                {
                    int tmpMax = cp[users[1] - 1].GetTractor();
                    if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                    {
                        order = users[1];
                        orderMax = tmpMax;
                    }
                }
                if (cp[users[2] - 1].HasTractors())
                {
                    int tmpMax = cp[users[2] - 1].GetTractor();
                    if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                    {
                        order = users[2];
                        orderMax = tmpMax;
                    }
                }

                return order;
            }
            else if (cp[order - 1].GetPairs().Count == 1 && (count ==2))
            {
                //����е�����
                int orderMax = (int)cp[order - 1].GetPairs()[0];
                if (cp[users[0] - 1].GetPairs().Count>0)
                {
                    int tmpMax = (int)cp[users[0] - 1].GetPairs()[0];
                    if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                    {
                        order = users[0];
                        orderMax = tmpMax;
                    }
                }
                if (cp[users[1] - 1].GetPairs().Count>0)
                {
                    int tmpMax = (int)cp[users[1] - 1].GetPairs()[0];
                    if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                    {
                        order = users[1];
                        orderMax = tmpMax;
                    }
                }
                if (cp[users[2] - 1].GetPairs().Count>0)
                {
                    int tmpMax = (int)cp[users[2] - 1].GetPairs()[0];
                    if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                    {
                        order = users[2];
                        orderMax = tmpMax;
                    }
                }

                return order;
            }
            else if (count == 1)
            {
                //����ǵ�����
                int orderMax = (int)mainForm.currentSendCards[order - 1][0];
                int tmpMax = (int)mainForm.currentSendCards[users[0] - 1][0]; 
                if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                {
                    order = users[0];
                    orderMax = tmpMax;
                }

                tmpMax = (int)mainForm.currentSendCards[users[1] - 1][0]; 
                if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                {
                    order = users[1];
                    orderMax = tmpMax;
                }
                tmpMax = (int)mainForm.currentSendCards[users[2] - 1][0]; 
                if (!CommonMethods.CompareTo(orderMax, tmpMax, suit, rank, firstSuit))
                {
                    order = users[2];
                    orderMax = tmpMax;
                }

                return order;
            }

            return order;
        }


        //����ÿ�εĵ÷�
        internal static void CalculateScore(MainForm mainForm)
        {
            int score = 0;

            score += GetScores(mainForm.currentSendCards[0]);
            score += GetScores(mainForm.currentSendCards[1]);
            score += GetScores(mainForm.currentSendCards[2]);
            score += GetScores(mainForm.currentSendCards[3]);

            mainForm.Scores += score;

            //mainForm.Text = mainForm.Scores + "";
        }

        //�õ����Ƶķ���
        internal static void Calculate8CardsScore(MainForm mainForm,int howmany)
        {
            int score = GetScores(mainForm.send8Cards);

            score = score * howmany;
            mainForm.Scores += score;
   
        }

        private static int GetScores(ArrayList list)
        {
            int number = 0;
            int score = 0;

            for (int i = 0; i < list.Count; i++)
            {
                number = (int)list[i] % 13;
                if (number == 3)
                {
                    score += 5;
                }
                else if (number == 8)
                {
                    score += 10;
                }
                else if (number == 11)
                {
                    score += 10;
                }
            }
            return score;
        }

        //���˦��ʱ�ļ��,������е��ƶ������ģ�true
        internal static bool CheckSendCards(MainForm mainForm, ArrayList minCards,int who)
        {
            //ArrayList minCards = new ArrayList();
            int[] users = CommonMethods.OtherUsers(who);

            ArrayList list = new ArrayList();
            CurrentPoker cp = new CurrentPoker();
            int suit = mainForm.currentState.Suit;
            int rank = mainForm.currentRank;
            cp.Suit = suit;
            cp.Rank = rank;

            
            for (int i = 0; i < mainForm.myCardIsReady.Count; i++)
            {
                if ((bool)mainForm.myCardIsReady[i])
                {
                    list.Add(mainForm.myCardsNumber[i]);
                }
            }

            int firstSuit = CommonMethods.GetSuit((int)list[0],cp.Suit,cp.Rank);

            cp = CommonMethods.parse(list, cp.Suit, cp.Rank);
            cp.Sort();

           

            if (list.Count == 1) //����ǵ�����
            {
                return true;
            }
            else if (list.Count == 2 && (cp.GetPairs().Count == 1)) //�����һ��
            {
                return true;
            }
            else if (list.Count == 4 && (cp.HasTractors())) //�����������
            {
                return true;
            }
            else //��˦�����ʱ
            {
                if (cp.HasTractors())
                {
                    int myMax = cp.GetTractor();
                    int[] ttt = cp.GetTractorOtherCards(myMax);
                    cp.RemoveCard(myMax);
                    cp.RemoveCard(myMax);
                    cp.RemoveCard(ttt[1]);
                    cp.RemoveCard(ttt[1]);

                    int[] myMaxs = cp.GetTractorOtherCards(myMax);
                    int max4 = mainForm.currentPokers[users[0]].GetTractor(firstSuit);
                    int max2 = mainForm.currentPokers[users[1]].GetTractor(firstSuit);
                    int max3 = mainForm.currentPokers[users[2]].GetTractor(firstSuit);
                    if (!CommonMethods.CompareTo(myMax, max2, suit, rank, firstSuit))
                    {
                        minCards.Add(myMax);
                        minCards.Add(myMax);
                        minCards.Add(ttt[1]);
                        minCards.Add(ttt[1]);
                        return false;
                    }
                    else if (!CommonMethods.CompareTo(myMax, max3, suit, rank, firstSuit))
                    {
                        minCards.Add(myMax);
                        minCards.Add(myMax);
                        minCards.Add(ttt[1]);
                        minCards.Add(ttt[1]);
                        return false;
                    }
                    else if (!CommonMethods.CompareTo(myMax, max4, suit, rank, firstSuit))
                    {
                        minCards.Add(myMax);
                        minCards.Add(myMax);
                        minCards.Add(ttt[1]);
                        minCards.Add(ttt[1]);
                        return false;
                    }
                }

                if (cp.GetPairs().Count>0)
                {
                    ArrayList list0 = cp.GetPairs();

                    ArrayList list4 = mainForm.currentPokers[users[0]].GetPairs(firstSuit);
                    ArrayList list2 = mainForm.currentPokers[users[1]].GetPairs(firstSuit);
                    ArrayList list3 = mainForm.currentPokers[users[2]].GetPairs(firstSuit);

                    
                    int max4 = -1;
                    int max2 = -1;
                    int max3 = -1;
                    if (list4.Count > 0)
                    {
                        max4 = (int)list4[list4.Count - 1];
                    }
                    if (list3.Count > 0)
                    {
                        max3 = (int)list3[list3.Count - 1];
                    }

                    if (list2.Count > 0)
                    {
                        max2 = (int)list2[list2.Count - 1];
                    }

                    

                    for (int i = 0; i < list0.Count; i++)
                    {
                        int myMax = (int)list0[i];
                        cp.RemoveCard(myMax);
                        cp.RemoveCard(myMax);

                        if (!CommonMethods.CompareTo(myMax, max2, suit, rank, firstSuit) && max2 > -1)
                        {
                            minCards.Add(myMax);
                            minCards.Add(myMax);
                            return false;
                        }
                        else if (!CommonMethods.CompareTo(myMax, max3, suit, rank, firstSuit) && max3 > -1)
                        {
                            minCards.Add(myMax);
                            minCards.Add(myMax);
                            return false;
                        }
                        else if (!CommonMethods.CompareTo(myMax, max4, suit, rank, firstSuit) && max4 > -1)
                        {
                            minCards.Add(myMax);
                            minCards.Add(myMax);
                            return false;
                        }
                    }

                }

                //���μ��ÿ�����Ƿ������
                int[] cards = cp.GetCards();
                int mmax4 = mainForm.currentPokers[users[0]].GetMaxCard(firstSuit);
                int mmax2 = mainForm.currentPokers[users[1]].GetMaxCard(firstSuit);
                int mmax3 = mainForm.currentPokers[users[2]].GetMaxCard(firstSuit);
                for (int i = 0; i < 54; i++)
                {
                    if (cards[i] == 1)
                    {
                        if (!CommonMethods.CompareTo(i, mmax2, suit, rank, firstSuit))
                        {
                            minCards.Add(i);
                            return false;
                        }
                        else if (!CommonMethods.CompareTo(i, mmax3, suit, rank, firstSuit))
                        {
                            minCards.Add(i);
                            return false;
                        }
                        else if (!CommonMethods.CompareTo(i, mmax4, suit, rank, firstSuit))
                        {
                            minCards.Add(i);
                            return false;
                        }
                    }
                }
            }

            return true;
        }


        internal static bool CheckSendCards(MainForm mainForm, ArrayList sendCards,ArrayList minCards, int who)
        {
            //ArrayList minCards = new ArrayList();
            int[] users = CommonMethods.OtherUsers(who);

            ArrayList list = new ArrayList();
            CurrentPoker cp = new CurrentPoker();
            int suit = mainForm.currentState.Suit;
            int rank = mainForm.currentRank;
            cp.Suit = suit;
            cp.Rank = rank;


            for (int i = 0; i < sendCards.Count; i++)
            {
                 list.Add(sendCards[i]);
                
            }

            int firstSuit = CommonMethods.GetSuit((int)list[0], cp.Suit, cp.Rank);

            cp = CommonMethods.parse(list, cp.Suit, cp.Rank);
            cp.Sort();



            if (list.Count == 1) //����ǵ�����
            {
                return true;
            }
            else if (list.Count == 2 && (cp.GetPairs().Count == 1)) //�����һ��
            {
                return true;
            }
            else if (list.Count == 4 && (cp.HasTractors())) //�����������
            {
                return true;
            }
            else //��˦�����ʱ
            {
                if (cp.HasTractors())
                {
                    int myMax = cp.GetTractor();
                    int[] ttt = cp.GetTractorOtherCards(myMax);
                    cp.RemoveCard(myMax);
                    cp.RemoveCard(myMax);
                    cp.RemoveCard(ttt[1]);
                    cp.RemoveCard(ttt[1]);

                    int[] myMaxs = cp.GetTractorOtherCards(myMax);
                    int max4 = mainForm.currentPokers[users[0]].GetTractor(firstSuit);
                    int max2 = mainForm.currentPokers[users[1]].GetTractor(firstSuit);
                    int max3 = mainForm.currentPokers[users[2]].GetTractor(firstSuit);
                    if (!CommonMethods.CompareTo(myMax, max2, suit, rank, firstSuit))
                    {
                        minCards.Add(myMax);
                        minCards.Add(myMax);
                        minCards.Add(ttt[1]);
                        minCards.Add(ttt[1]);
                        return false;
                    }
                    else if (!CommonMethods.CompareTo(myMax, max3, suit, rank, firstSuit))
                    {
                        minCards.Add(myMax);
                        minCards.Add(myMax);
                        minCards.Add(ttt[1]);
                        minCards.Add(ttt[1]);
                        return false;
                    }
                    else if (!CommonMethods.CompareTo(myMax, max4, suit, rank, firstSuit))
                    {
                        minCards.Add(myMax);
                        minCards.Add(myMax);
                        minCards.Add(ttt[1]);
                        minCards.Add(ttt[1]);
                        return false;
                    }
                }

                if (cp.GetPairs().Count > 0)
                {
                    ArrayList list0 = cp.GetPairs();

                    ArrayList list4 = mainForm.currentPokers[users[0]].GetPairs(firstSuit);
                    ArrayList list2 = mainForm.currentPokers[users[1]].GetPairs(firstSuit);
                    ArrayList list3 = mainForm.currentPokers[users[2]].GetPairs(firstSuit);


                    int max4 = -1;
                    int max2 = -1;
                    int max3 = -1;
                    if (list4.Count > 0)
                    {
                        max4 = (int)list4[list4.Count - 1];
                    }
                    if (list3.Count > 0)
                    {
                        max3 = (int)list3[list3.Count - 1];
                    }

                    if (list2.Count > 0)
                    {
                        max2 = (int)list2[list2.Count - 1];
                    }



                    for (int i = 0; i < list0.Count; i++)
                    {
                        int myMax = (int)list0[i];
                        cp.RemoveCard(myMax);
                        cp.RemoveCard(myMax);

                        if (!CommonMethods.CompareTo(myMax, max2, suit, rank, firstSuit) && max2 > -1)
                        {
                            minCards.Add(myMax);
                            minCards.Add(myMax);
                            return false;
                        }
                        else if (!CommonMethods.CompareTo(myMax, max3, suit, rank, firstSuit) && max3 > -1)
                        {
                            minCards.Add(myMax);
                            minCards.Add(myMax);
                            return false;
                        }
                        else if (!CommonMethods.CompareTo(myMax, max4, suit, rank, firstSuit) && max4 > -1)
                        {
                            minCards.Add(myMax);
                            minCards.Add(myMax);
                            return false;
                        }
                    }

                }

                //���μ��ÿ�����Ƿ������
                int[] cards = cp.GetCards();
                int mmax4 = mainForm.currentPokers[users[0]].GetMaxCard(firstSuit);
                int mmax2 = mainForm.currentPokers[users[1]].GetMaxCard(firstSuit);
                int mmax3 = mainForm.currentPokers[users[2]].GetMaxCard(firstSuit);
                for (int i = 0; i < 54; i++)
                {
                    if (cards[i] == 1)
                    {
                        if (!CommonMethods.CompareTo(i, mmax2, suit, rank, firstSuit))
                        {
                            minCards.Add(i);
                            return false;
                        }
                        else if (!CommonMethods.CompareTo(i, mmax3, suit, rank, firstSuit))
                        {
                            minCards.Add(i);
                            return false;
                        }
                        else if (!CommonMethods.CompareTo(i, mmax4, suit, rank, firstSuit))
                        {
                            minCards.Add(i);
                            return false;
                        }
                    }
                }
            }

            return true;
        }

    }
}
