using System;
using System.Collections;
using System.Text;

namespace Kuaff.Tractor
{
    /// <summary>
    /// ��θ��Ƶ��㷨
    /// </summary>
    class MustSendCardsAlgorithm
    {
        /// <summary>
        /// ���ƻ����㷨
        /// </summary>
        /// <param name="mainForm">������</param>
        /// <param name="currentPokers">��ǰ�������е��˿�</param>
        /// <param name="whoseOrder">��˭����</param>
        /// <param name="sendedCards">whoseOrderӦ�ó�����</param>
        /// <param name="count">��������</param>
        internal static void MustSendCards(MainForm mainForm, CurrentPoker[] currentPokers, int whoseOrder, ArrayList sendedCards, int count)
        {
            //��ǰ�Ļ�ɫ��Rank
            int suit = mainForm.currentState.Suit;
            int rank = mainForm.currentRank;

            //���λ�ɫ
            int firstSuit = CommonMethods.GetSuit((int)mainForm.currentSendCards[mainForm.firstSend-1][0],suit,rank);

            int sendTotal = mainForm.currentSendCards[0].Count + mainForm.currentSendCards[1].Count + mainForm.currentSendCards[2].Count + mainForm.currentSendCards[3].Count;

            if (sendTotal == count) //whoseOrder�ǵڶ�������
            {
                WhoseOrderIs2(mainForm, currentPokers, whoseOrder, sendedCards, count, suit, rank, firstSuit);
                
            }
            else if (sendTotal == count*2) //whoseOrder�ǵ���������
            {
                WhoseOrderIs3(mainForm, currentPokers, whoseOrder, sendedCards, count, suit, rank, firstSuit);
                
            }
            else if (sendTotal == count * 3) //whoseOrder�ǵ��ĸ�����
            {
                WhoseOrderIs4(mainForm, currentPokers, whoseOrder, sendedCards, count, suit, rank, firstSuit);
               
            }
        }

        //whoseOrder�ǵڶ�������
        internal static void WhoseOrderIs2(MainForm mainForm, CurrentPoker[] currentPokers, int whoseOrder, ArrayList sendedCards, int count, int suit, int rank, int firstSuit)
        {
            ArrayList firstSendCards = mainForm.currentSendCards[mainForm.firstSend-1]; //�׼ҳ���
            CurrentPoker firstCP = new CurrentPoker();
            firstCP.Suit = suit;
            firstCP.Rank = rank;
            firstCP = CommonMethods.parse(firstSendCards, suit, rank);
            
            int firstMax = CommonMethods.GetMaxCard(firstSendCards, suit, rank); //�׼ҵ������
            int pairTotal = firstCP.GetPairs().Count;

            CurrentPoker myCP = currentPokers[whoseOrder - 1];
            ArrayList myPokerList = mainForm.pokerList[whoseOrder - 1];


            //whose�Ĵ˻�ɫ������
            int myTotal = CommonMethods.GetSuitCount(currentPokers[whoseOrder - 1], suit, rank, firstSuit); 
            //�˻�ɫ����
            int[] cards = myCP.GetSuitCards(firstSuit);

            

            ArrayList myList = new ArrayList(cards);
            CurrentPoker mySuitCP = new CurrentPoker(); //�Ҵ˻�ɫ����
            mySuitCP.Suit = suit;
            mySuitCP.Rank = rank;
            mySuitCP = CommonMethods.parse(myList,suit,rank);
            mySuitCP.Sort();

            firstCP.Sort();

            myCP.Sort();


           //���Ǳ�
            if (myTotal == 0)
            {
                if (firstSuit != suit)
                {

                    if (myCP.GetMasterCardsTotal() >= count && count == 1) //������
                    {
                        //���Ŀǰ������һ������ 
                        int biggerMax = (int)mainForm.currentSendCards[mainForm.whoIsBigger - 1][0];
                        int maxMaster = myCP.GetMaxMasterCards();
                        //����ҵ����ܴ�������Ǽҵ���
                        if (!CommonMethods.CompareTo(biggerMax, maxMaster, suit, rank, firstSuit))
                        {
                            mainForm.whoIsBigger = whoseOrder;
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, maxMaster);
                            return;
                        }
                    }
                    else if (myCP.GetMasterCardsTotal() >= count && pairTotal == 1 && count == 2) //��һ����ʱ
                    {
                        //���Ŀǰ������һ������ 
                        int biggerMax = (int)mainForm.currentSendCards[mainForm.whoIsBigger - 1][0];
                        ArrayList masterPairs  = myCP.GetMasterPairs();
                        //����ҵ����ܴ�������Ǽҵ���
                        if (masterPairs.Count > 0)
                        {
                            mainForm.whoIsBigger = whoseOrder;
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)masterPairs[0]);
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)masterPairs[0]);
                            return;
                        }
                    }
                    else if (myCP.GetMasterCardsTotal() >= count && pairTotal == 0 && count > 1) //����˦��
                    {
                        //���Ŀǰ������һ������ 
                        int biggerMax = (int)mainForm.currentSendCards[mainForm.whoIsBigger - 1][0];
                        int maxMaster = myCP.GetMaxMasterCards();
                        //����ҵ����ܴ�������Ǽҵ���
                        if (!CommonMethods.CompareTo(biggerMax, maxMaster, suit, rank, firstSuit))
                        {
                            mainForm.whoIsBigger = whoseOrder;
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, maxMaster);

                            SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, true);
                            SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, true);
                            SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, false);
                            SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, false);

                            return;
                        }

                    }
                }
            }

            if (myTotal < count) //����ɫ����
            {
               
                for (int i = 0; i < myTotal; i++)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, cards[i]);
                }

                SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList,true);
                SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList,true);
                SendMasterSuitNoScores(sendedCards, count, suit, myCP, myPokerList,true);
                SendMasterSuit(sendedCards, count, suit, myCP, myPokerList,true);
                SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList, false);
                SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, false);
                SendMasterSuitNoScores(sendedCards, count, suit, myCP, myPokerList, false);
                SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, false);

               
                return;

            }
          
            //����ȷ���˻�ɫ���ƾ��Թ���
            else if (firstCP.HasTractors())  //����׼ҳ���������
            {
                //���������������������������
                if (mySuitCP.HasTractors())
                {
                    int k = mySuitCP.GetTractor();
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, k);
                    int[] ks = mySuitCP.GetTractorOtherCards(k);
                    for (int i = 0; i < 3; i++)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, ks[i]);
                    }

                    if (!CommonMethods.CompareTo(firstCP.GetTractor(), k, suit, rank, firstSuit)) //����ҵ����������ƴ�
                    {
                        mainForm.whoIsBigger = whoseOrder;
                    }
                }
                else if (mySuitCP.GetPairs().Count > 0) //����жԣ���������
                {
                    ArrayList list = mySuitCP.GetPairs();
                    if (list.Count >= 2)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[1]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[1]);
                    }
                    else
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                    }

                }


                //�������С����
                SendThisSuitNoScores(sendedCards, count, suit,firstSuit, myCP, myPokerList,true);
                SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);

                SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                
                return;
            }
            else if (count == 1) //�׼ҳ������� 
            {
                int myMax = -1;
                if (firstSuit == suit)
                {
                    myMax = mySuitCP.GetMaxMasterCards();
                }
                else
                {
                    myMax = mySuitCP.GetMaxCards(firstSuit);
                }

               

                //����õ��Ĵ˻�ɫ�������ƴ����׼ҵ���
                if (!CommonMethods.CompareTo(firstMax,myMax,suit,rank,firstSuit))
                {
                    if (myMax > -1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myMax);

                        mainForm.whoIsBigger = whoseOrder;
                        
                        return;
                    }
                }

                


                SendThisSuitNoScores(sendedCards, count, suit,firstSuit, myCP, myPokerList,true);
                SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);

                SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

               
                return;
            }
            else if ((pairTotal == 1) && (count == 2)) //�׼ҳ���һ����
            {
                ArrayList list = mySuitCP.GetPairs();
                if (list.Count >= 1)
                {
                    int myMax = (int)list[list.Count - 1];

                    //����õ��Ĵ˻�ɫ�������ƴ����׼ҵ���
                    if (!CommonMethods.CompareTo(firstMax, myMax, suit, rank, firstSuit))
                    {
                        mainForm.whoIsBigger = whoseOrder;
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[list.Count - 1]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[list.Count - 1]);
                        
                        return;
                    }
                    else
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);

                        
                        return;
                    }
                }
                else
                {
                    //�������С����
                    SendThisSuitNoScores(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                    SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);

                    SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                    SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                    
                    return;
                }

            }
            else if (count == pairTotal * 2 && (count>0)) //���Ƕ�
            {
                ArrayList list = mySuitCP.GetPairs();
                for (int i = 0; i < pairTotal && i < list.Count;i++ )
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[i]);
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[i]);
                }

                //�������С����
                SendThisSuitNoScores(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);

                SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                
                return;

            }
            else //�жԺ��е����ƣ���˦��
            {
                ArrayList list = mySuitCP.GetPairs();
                for (int i = 0; i < pairTotal && i < list.Count; i++)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[i]);
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[i]);
                }

                //�������С����
                SendThisSuitNoScores(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);

                SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

               
                return;
            }


        }

        private static void SendOtherSuitNoScores(ArrayList sendedCards, int count, int firstSuit, CurrentPoker myCP, ArrayList myPokerList,bool protectPairs)
        {


            for (int asuit = 1; asuit < 5; asuit++)
            {
                if (asuit == firstSuit)
                {
                    continue;
                }
                if (asuit == myCP.Suit)
                {
                    continue;
                }


                int[] cards = myCP.GetSuitAllCards(asuit);

                for (int m = 0; m < 13; m++)
                {
                    if (m == 3 || m == 8 || m ==11)
                    {
                        continue;
                    }

                    if (cards[m] > 0)
                    {
                        if (sendedCards.Count < count)
                        {
                            if (protectPairs) //������
                            {
                                if (cards[m] == 1)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                }
                            }
                            else
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                if (sendedCards.Count < count)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                }
                            }
                        }
                        if (sendedCards.Count >= count)
                        {
                            return;
                        }
                    }
                }

            }



               
        }

        private static void SendOtherSuitOrScores(ArrayList sendedCards, int count, int firstSuit, CurrentPoker myCP, ArrayList myPokerList, bool protectPairs)
        {
            for (int asuit = 1; asuit < 5; asuit++)
            {
                if (asuit == firstSuit)
                {
                    continue;
                }
                if (asuit == myCP.Suit)
                {
                    continue;
                }


                int[] cards = myCP.GetSuitAllCards(asuit);

                int n = 8;
                if (cards[n] > 0)
                {
                    if (sendedCards.Count < count)
                    {
                        if (protectPairs) //������
                        {
                            if (cards[n] == 1)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            }
                        }
                        else
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            if (sendedCards.Count < count)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                                if (sendedCards.Count < count)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                                }
                            }
                        }
                    }
                }
                n = 11;

                if (cards[n] > 0)
                {
                    if (sendedCards.Count < count)
                    {
                        if (protectPairs) //������
                        {
                            if (cards[n] == 1)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            }
                        }
                        else
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            if (sendedCards.Count < count)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                                if (sendedCards.Count < count)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                                }
                            }
                        }
                    }
                }


                n = 5;

                if (cards[n] > 0)
                {
                    if (sendedCards.Count < count)
                    {
                        if (protectPairs) //������
                        {
                            if (cards[n] == 1)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            }
                        }
                        else
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            if (sendedCards.Count < count)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            }
                        }
                    }
                }

                for (int m = 0; m < 13; m++)
                {
                    if (m == 3 || m == 8 || m == 11)
                    {
                        continue;
                    }

                    if (cards[m] > 0)
                    {
                        if (sendedCards.Count < count)
                        {
                            if (protectPairs) //������
                            {
                                if (cards[m] == 1)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                }
                            }
                            else
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                if (sendedCards.Count < count)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                }
                            }
                        }
                        if (sendedCards.Count >= count)
                        {
                            return;
                        }
                    }
                }

            }


        }

        private static void SendOtherSuit(ArrayList sendedCards, int count, int firstSuit, CurrentPoker myCP, ArrayList myPokerList, bool protectPairs)
        {
            for (int asuit = 1; asuit < 5; asuit++)
            {
                if (asuit == firstSuit)
                {
                    continue;
                }
                if (asuit == myCP.Suit)
                {
                    continue;
                }


                int[] cards = myCP.GetSuitAllCards(asuit);

                for (int m = 0; m < 13; m++)
                {
                    if (cards[m] > 0)
                    {
                        if (sendedCards.Count < count)
                        {
                            if (protectPairs) //������
                            {
                                if (cards[m] == 1)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                }
                            }
                            else
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                if (sendedCards.Count < count)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                }
                            }
                        }
                        if (sendedCards.Count >= count)
                        {
                            return;
                        }
                    }
                }

            }


        }


        private static void SendThisSuitNoScores(ArrayList sendedCards, int count, int suit, int firstSuit, CurrentPoker myCP, ArrayList myPokerList, bool protectPairs)
        {
            if (suit == firstSuit)
            {
                SendMasterSuitNoScores(sendedCards, count, suit, myCP, myPokerList, protectPairs);
                return;
            }

            int[] cards = myCP.GetSuitAllCards(firstSuit);

            for (int m = 0; m < 13; m++)
            {
                if (m == 3 || m == 8 || m == 11)
                {
                    continue;
                }

                if (cards[m] > 0)
                {
                    if (sendedCards.Count < count)
                    {
                        if (protectPairs) //������
                        {
                            if (cards[m] == 1)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (firstSuit - 1) * 13);
                            }
                        }
                        else
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (firstSuit - 1) * 13);
                            if (sendedCards.Count < count)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (firstSuit - 1) * 13);
                            }
                        }

                    }
                    if (sendedCards.Count >= count)
                    {
                        return;
                    }
                }
            }

        }

        private static void SendThisSuitOrScores(ArrayList sendedCards, int count, int suit, int firstSuit, CurrentPoker myCP, ArrayList myPokerList, bool protectPairs)
        {
            if (suit == firstSuit)
            {
                SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList,protectPairs);
                return;
            }

            int[] cards = myCP.GetSuitAllCards(firstSuit);

            int n = 8;
            if (cards[n] > 0)
            {
                if (sendedCards.Count < count)
                {
                    if (protectPairs) //������
                    {
                        if (cards[n] == 1)
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (firstSuit - 1) * 13);
                        }
                    }
                    else
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (firstSuit - 1) * 13);
                        if (sendedCards.Count < count)
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (firstSuit - 1) * 13);
                        }
                    }

                }
                if (sendedCards.Count >= count)
                {
                    return;
                }
            }

            n = 11;
            if (cards[n] > 0)
            {
                if (sendedCards.Count < count)
                {
                    if (protectPairs) //������
                    {
                        if (cards[n] == 1)
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (firstSuit - 1) * 13);
                        }
                    }
                    else
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (firstSuit - 1) * 13);
                        if (sendedCards.Count < count)
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (firstSuit - 1) * 13);
                        }
                    }

                }
                if (sendedCards.Count >= count)
                {
                    return;
                }
            }
            n = 3;
            if (cards[n] > 0)
            {
                if (sendedCards.Count < count)
                {
                    if (protectPairs) //������
                    {
                        if (cards[n] == 1)
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (firstSuit - 1) * 13);
                        }
                    }
                    else
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (firstSuit - 1) * 13);
                        if (sendedCards.Count < count)
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (firstSuit - 1) * 13);
                        }
                    }

                }
                if (sendedCards.Count >= count)
                {
                    return;
                }
            }



            for (int m = 0; m < 13; m++)
            {
               
                if (cards[m] > 0)
                {
                    if (sendedCards.Count < count)
                    {
                        if (protectPairs) //������
                        {
                            if (cards[m] == 1)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (firstSuit - 1) * 13);
                            }
                        }
                        else
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (firstSuit - 1) * 13);
                            if (sendedCards.Count < count)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (firstSuit - 1) * 13);
                            }
                        }

                    }
                    if (sendedCards.Count >= count)
                    {
                        return;
                    }
                }
            }

        }

        private static void SendThisSuit(ArrayList sendedCards, int count, int suit, int firstSuit, CurrentPoker myCP, ArrayList myPokerList, bool protectPairs)
        {
            if (suit == firstSuit)
            {
                SendMasterSuit(sendedCards, count, suit, myCP, myPokerList,protectPairs);
                return;
            }

            int[] cards = myCP.GetSuitAllCards(firstSuit);

            for (int m = 0; m < 13; m++)
            {
               
                if (cards[m] > 0)
                {
                    if (sendedCards.Count < count)
                    {
                        if (protectPairs) //������
                        {
                            if (cards[m] == 1)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (firstSuit - 1) * 13);
                            }
                        }
                        else
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (firstSuit - 1) * 13);
                            if (sendedCards.Count < count)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (firstSuit - 1) * 13);
                            }
                        }

                    }
                    if (sendedCards.Count >= count)
                    {
                        return;
                    }
                }
            }

        }


        private static void SendMasterSuitNoScores(ArrayList sendedCards, int count, int suit, CurrentPoker myCP, ArrayList myPokerList, bool protectPairs)
        {

            if (sendedCards.Count >= count)
            {
                return;
            }

            for (int asuit = 1; asuit < 5; asuit++)
            {
                if (asuit != suit)
                {
                    continue;
                }

                int[] cardsCount = myCP.GetSuitAllCards(asuit);


                for (int m = 0; m < 13; m++)
                {
                    if (m == 3 || m == 8 || m == 11)
                    {
                        continue;
                    }

                    if (cardsCount[m] > 0)
                    {
                        if (sendedCards.Count < count)
                        {
                            if (protectPairs) //������
                            {
                                if (cardsCount[m] == 1)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (suit - 1) * 13);
                                }
                            }
                            else
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                if (sendedCards.Count < count)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                }
                            }
                        }
                        if (sendedCards.Count >= count)
                        {
                            return;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }



            if (protectPairs) //������
            {
                if (sendedCards.Count < count)
                {
                    if (myCP.HeartsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.PeachsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 13);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.DiamondsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 26);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.ClubsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 39);
                    }
                }
            }
            else
            {
                if (sendedCards.Count < count)
                {
                    if (myCP.HeartsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.HeartsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.PeachsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 13);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.PeachsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 13);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.DiamondsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 26);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.DiamondsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 26);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.ClubsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 39);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.ClubsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 39);
                    }
                }
            }

            //
            if (myCP.SmallJack > 0)
            {
                if (sendedCards.Count < count)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, 52);

                }
            }
            if (myCP.SmallJack > 0)
            {
                if (sendedCards.Count < count)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, 52);

                }
            }
            if (myCP.BigJack > 0)
            {
                if (sendedCards.Count < count)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, 53);

                }
            }
            if (myCP.BigJack > 0)
            {
                if (sendedCards.Count < count)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, 53);

                }
            }
        }

        private static void SendMasterSuitOrScores(ArrayList sendedCards, int count, int suit, CurrentPoker myCP, ArrayList myPokerList, bool protectPairs)
        {
            if (sendedCards.Count >= count)
            {
                return;
            }

            for (int asuit = 1; asuit < 5; asuit++)
            {
                if (asuit != suit)
                {
                    continue;
                }

                int[] cardsCount = myCP.GetSuitAllCards(asuit);

                

                int n = 8;

                if (myCP.Rank == 8)
                {
                    n = 11; //��10ʱ���ȳ�K
                }

                if (cardsCount[n] > 0)
                {
                    if (sendedCards.Count < count)
                    {
                        if (protectPairs) //������
                        {
                            if (cardsCount[n] == 1)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (suit - 1) * 13);
                            }
                        }
                        else
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            if (sendedCards.Count < count)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            }
                        }
                    }
                    if (sendedCards.Count >= count)
                    {
                        return;
                    }
                }
                else
                {
                    continue;
                }

                n = 11;

                if (myCP.Rank == 8)
                {
                    n = 8; //��10ʱ�����10
                }

                if (cardsCount[n] > 0)
                {
                    if (sendedCards.Count < count)
                    {
                        if (protectPairs) //������
                        {
                            if (cardsCount[n] == 1)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (suit - 1) * 13);
                            }
                        }
                        else
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            if (sendedCards.Count < count)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            }
                        }
                    }
                    if (sendedCards.Count >= count)
                    {
                        return;
                    }
                }
                else
                {
                    continue;
                }

                n = 3;

                if (cardsCount[n] > 0)
                {
                    if (sendedCards.Count < count)
                    {
                        if (protectPairs) //������
                        {
                            if (cardsCount[n] == 1)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (suit - 1) * 13);
                            }
                        }
                        else
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            if (sendedCards.Count < count)
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, n + (asuit - 1) * 13);
                            }
                        }
                    }
                    if (sendedCards.Count >= count)
                    {
                        return;
                    }
                }
                else
                {
                    continue;
                }
            }
            

            //�Ƿ�
            for (int asuit = 1; asuit < 5; asuit++)
            {
                if (asuit != suit)
                {
                    continue;
                }

                int[] cardsCount = myCP.GetSuitAllCards(asuit);


                for (int m = 0; m < 13; m++)
                {
                    

                    if (cardsCount[m] > 0)
                    {
                        if (sendedCards.Count < count)
                        {
                            if (protectPairs) //������
                            {
                                if (cardsCount[m] == 1)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (suit - 1) * 13);
                                }
                            }
                            else
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                if (sendedCards.Count < count)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                }
                            }
                        }
                        if (sendedCards.Count >= count)
                        {
                            return;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            //Rank
            if (protectPairs) //������
            {
                if (sendedCards.Count < count)
                {
                    if (myCP.HeartsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.PeachsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 13);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.DiamondsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 26);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.ClubsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 39);
                    }
                }
            }
            else
            {
                if (sendedCards.Count < count)
                {
                    if (myCP.HeartsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.HeartsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.PeachsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 13);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.PeachsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 13);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.DiamondsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 26);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.DiamondsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 26);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.ClubsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 39);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.ClubsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 39);
                    }
                }
            }
           
            //
            if (myCP.SmallJack > 0)
            {
                if (sendedCards.Count < count)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, 52);

                }
            }
            if (myCP.SmallJack > 0)
            {
                if (sendedCards.Count < count)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, 52);

                }
            }
            if (myCP.BigJack > 0)
            {
                if (sendedCards.Count < count)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, 53);

                }
            }
            if (myCP.BigJack > 0)
            {
                if (sendedCards.Count < count)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, 53);

                }
            }
        }

        private static void SendMasterSuit(ArrayList sendedCards, int count, int suit, CurrentPoker myCP, ArrayList myPokerList, bool protectPairs)
        {
            if (sendedCards.Count >= count)
            {
                return;
            }

            for (int asuit = 1; asuit < 5; asuit++)
            {
                if (asuit != suit)
                {
                    continue;
                }

                int[] cardsCount = myCP.GetSuitAllCards(asuit);


                for (int m = 0; m < 13; m++)
                {
                   

                    if (cardsCount[m] > 0)
                    {
                        if (sendedCards.Count < count)
                        {
                            if (protectPairs) //������
                            {
                                if (cardsCount[m] == 1)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (suit - 1) * 13);
                                }
                            }
                            else
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                if (sendedCards.Count < count)
                                {
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, m + (asuit - 1) * 13);
                                }
                            }
                        }
                        if (sendedCards.Count >= count)
                        {
                            return;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
            }



            if (protectPairs) //������
            {
                if (sendedCards.Count < count)
                {
                    if (myCP.HeartsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.PeachsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 13);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.DiamondsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 26);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.ClubsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 39);
                    }
                }
            }
            else
            {
                if (sendedCards.Count < count)
                {
                    if (myCP.HeartsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.HeartsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.PeachsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 13);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.PeachsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 13);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.DiamondsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 26);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.DiamondsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 26);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.ClubsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 39);
                    }
                }
                if (sendedCards.Count < count)
                {
                    if (myCP.ClubsRankTotal > 0)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myCP.Rank + 39);
                    }
                }
            }
            
            //
            if (myCP.SmallJack > 0)
            {
                if (sendedCards.Count < count)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, 52);

                }
            }
            if (myCP.SmallJack > 0)
            {
                if (sendedCards.Count < count)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, 52);

                }
            }
            if (myCP.BigJack > 0)
            {
                if (sendedCards.Count < count)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, 53);

                }
            }
            if (myCP.BigJack > 0)
            {
                if (sendedCards.Count < count)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, 53);

                }
            }
        }


        //whoseOrder�ǵ���������
        internal static void WhoseOrderIs3(MainForm mainForm, CurrentPoker[] currentPokers, int whoseOrder, ArrayList sendedCards, int count, int suit, int rank, int firstSuit)
        {
            ArrayList firstSendCards = mainForm.currentSendCards[mainForm.firstSend - 1]; //�׼ҳ�����
            CurrentPoker firstCP = new CurrentPoker();
            firstCP.Suit = suit;
            firstCP.Rank = rank;
            firstCP = CommonMethods.parse(firstSendCards, suit, rank);

            int firstMax = CommonMethods.GetMaxCard(firstSendCards, suit, rank); //�׼ҳ���������
            int pairTotal = firstCP.GetPairs().Count; //�׼ҳ��ĶԵ���Ŀ

            CurrentPoker myCP = currentPokers[whoseOrder - 1]; //�����е���
            ArrayList myPokerList = mainForm.pokerList[whoseOrder - 1];


            //whose�Ĵ˻�ɫ������
            int myTotal = CommonMethods.GetSuitCount(currentPokers[whoseOrder - 1], suit, rank, firstSuit);
            //�˻�ɫ����
            int[] cards = myCP.GetSuitCards(firstSuit);

           
            
            ArrayList myList = new ArrayList(cards);
            CurrentPoker mySuitCP = new CurrentPoker(); //�Ҵ˻�ɫ����
            mySuitCP.Suit = suit;
            mySuitCP.Rank = rank;
            mySuitCP = CommonMethods.parse(myList, suit, rank);
            mySuitCP.Sort();

            firstCP.Sort();
            myCP.Sort();

            int[] users = CommonMethods.OtherUsers(mainForm.firstSend);

            CurrentPoker secondCP = new CurrentPoker(); //�ڶ��ҳ�����
            secondCP.Suit = suit;
            secondCP.Rank = rank;
            secondCP = CommonMethods.parse(mainForm.currentSendCards[users[0]-1],suit,rank);

            //
            
            //�����Ƿ��
            //���������׼ҳ����ƵĴ�С��Ŀǰ���ܱ����
            if (myTotal == 0) 
            {
                if (firstSuit != suit)
                {
                    //���Ŀǰ������һ������ 
                    int biggerMax = (int)mainForm.currentSendCards[mainForm.whoIsBigger - 1][0];
                    int[] tmpUsers = CommonMethods.OtherUsers(whoseOrder);

                    if (myCP.GetMasterCardsTotal() >= count &&  (mainForm.whoIsBigger == tmpUsers[1]) && ((biggerMax % 13) > 8))
                    {
                        //���ϣ������п������ĸ���Ҳ�ȴ����һ�Ҵ�
                        SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ�Ƿ���
                        SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ����
                        SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ�Ƿ���
                        SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ����


                        int sendOtherSuitsTotal = sendedCards.Count; //û�и��ƿ�����ֻ�ܳ���

                        if (firstCP.HasTractors() && sendOtherSuitsTotal == 0) //������
                        {
                            int minMaster = myCP.GetMasterTractor();
                            int tmpFirstTractor = firstCP.GetTractor();

                            //����ҵ����ܴ�������Ǽҵ���

                            if ((!CommonMethods.CompareTo(tmpFirstTractor, minMaster, suit, rank, firstSuit)) && (minMaster > -1))
                            {
                                mainForm.whoIsBigger = whoseOrder;
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, minMaster);
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, minMaster);
                                int[] ttt = myCP.GetTractorOtherCards(minMaster);
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, ttt[1]);
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, ttt[1]);

                                return;
                            }
                        }
                        else if (myCP.GetMasterCardsTotal() >= count && count == 1 && sendOtherSuitsTotal ==0) //������
                        {
                            int maxMaster = myCP.GetMaxMasterCards();
                            //����ҵ����ܴ�������Ǽҵ���
                            if (!CommonMethods.CompareTo(biggerMax, maxMaster, suit, rank, firstSuit))
                            {
                                mainForm.whoIsBigger = whoseOrder;
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, maxMaster);
                                return;
                            }
                        }
                        else if (myCP.GetMasterCardsTotal() >= count &&  pairTotal == 1 && count == 2 && sendOtherSuitsTotal == 0) //��һ����ʱ
                        {

                            ArrayList masterPairs = myCP.GetMasterPairs();
                            //����ҵ����ܴ�������Ǽҵ���
                            if (masterPairs.Count > 0)
                            {
                                mainForm.whoIsBigger = whoseOrder;
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)masterPairs[0]);
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)masterPairs[0]);
                                return;
                            }
                        }
                        else if (myCP.GetMasterCardsTotal() >= count &&  pairTotal == 0 && count > 1 && sendOtherSuitsTotal == 0) //����˦��
                        {

                            int maxMaster = myCP.GetMaxMasterCards();
                            //����ҵ����ܴ�������Ǽҵ���
                            if (!CommonMethods.CompareTo(biggerMax, maxMaster, suit, rank, firstSuit))
                            {
                                mainForm.whoIsBigger = whoseOrder;
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, maxMaster);

                                SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, true);
                                SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, true);
                                SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, false);
                                SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, false);

                                return;
                            }

                        }

                        SendMasterSuitNoScores(sendedCards, count, suit, myCP, myPokerList, true); //���ƷǷ���
                        SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, true); //���Ʒ���
                        SendMasterSuitNoScores(sendedCards, count, suit, myCP, myPokerList, false); //���ƷǷ���
                        SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, false); //���Ʒ���

                    }
                    else
                    {
                        if (firstCP.HasTractors()) //��һ����ʱ
                        {
                            SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ�Ƿ���
                            SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ����
                            SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ�Ƿ���
                            SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ����
                        }
                        else if (myCP.GetMasterCardsTotal() >= count && count == 1) //������
                        {
                            
                            int maxMaster = myCP.GetMaxMasterCards();
                            //����ҵ����ܴ�������Ǽҵ���
                            if (!CommonMethods.CompareTo(biggerMax, maxMaster, suit, rank, firstSuit))
                            {
                                mainForm.whoIsBigger = whoseOrder;
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, maxMaster);
                                return;
                            }
                        }
                        else if (myCP.GetMasterCardsTotal() >= count &&  pairTotal == 1 && count == 2) //��һ����ʱ
                        {
                           
                            ArrayList masterPairs = myCP.GetMasterPairs();
                            //����ҵ����ܴ�������Ǽҵ���
                            if (masterPairs.Count > 0)
                            {
                                mainForm.whoIsBigger = whoseOrder;
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)masterPairs[0]);
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)masterPairs[0]);
                                return;
                            }
                        }
                        else if (myCP.GetMasterCardsTotal() >= count &&  pairTotal == 0 && count > 1) //����˦��
                        {
                           
                            int maxMaster = myCP.GetMaxMasterCards();
                            //����ҵ����ܴ�������Ǽҵ���
                            if (!CommonMethods.CompareTo(biggerMax, maxMaster, suit, rank, firstSuit))
                            {
                                mainForm.whoIsBigger = whoseOrder;
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, maxMaster);

                                SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, true);
                                SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, true);
                                SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, false);
                                SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, false);

                                return;
                            }

                        }
                    }
                }
            }
            if (myTotal < count) //����ɫ����
            {

                for (int i = 0; i < myTotal; i++)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, cards[i]);
                }

                SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList,true); //������ɫ�Ƿ���
                SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList,true); //������ɫ����
                SendMasterSuitNoScores(sendedCards, count, suit, myCP, myPokerList,true); //���ƷǷ���
                SendMasterSuit(sendedCards, count, suit, myCP, myPokerList,true); //���Ʒ���


                SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ�Ƿ���
                SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ����
                SendMasterSuitNoScores(sendedCards, count, suit, myCP, myPokerList, false); //���ƷǷ���
                SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, false); //���Ʒ���

              

                return;

            }
            
            else if (firstCP.HasTractors())  //����׼ҳ���������
            {
                //���������������������������
                if (mySuitCP.HasTractors())
                {
                    int k = mySuitCP.GetTractor();
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, k);
                    int[] ks = mySuitCP.GetTractorOtherCards(k);
                    for (int i = 0; i < 3; i++)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, ks[i]);
                    }


                    CurrentPoker tmpCP = CommonMethods.parse(mainForm.currentSendCards[mainForm.whoIsBigger - 1], suit, rank);
                    int tmp = tmpCP.GetTractor();
                    if (!CommonMethods.CompareTo(tmp, k, suit, rank, firstSuit))
                    {
                        mainForm.whoIsBigger = whoseOrder;
                    }
                    
                }
                else if (mySuitCP.GetPairs().Count > 0) //������жԣ���������
                {
                    ArrayList list = mySuitCP.GetPairs();
                    if (list.Count >= 2) //����������
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[1]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[1]);
                    }
                    else
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                    }


                }

                //�������С����
                if (mainForm.whoIsBigger == users[1])
                {
                    SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, true);
                    SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, true);

                    SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                    SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                }
                else
                {
                    SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, true);
                    SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, true);

                    SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                    SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                }

               
                return;

            }
            else if (count == 1) //�׼ҳ��˵����� 
            {
                int myMax = -1;  //�ҵĴ˻�ɫ�����ֵ
                if (firstSuit == suit)
                {
                    myMax = mySuitCP.GetMaxMasterCards();
                }
                else
                {
                    myMax = mySuitCP.GetMaxCards(firstSuit);
                }

                //�ڶ��������
                int max2 = CommonMethods.GetMaxCard(mainForm.currentSendCards[users[0]-1], suit, rank);

               

                //�׼Ҵ��ڵڶ���
                if (CommonMethods.CompareTo(firstMax, max2, suit, rank,firstSuit))
                {
                    //������ļ��б��׼Ҵ���ƣ���Ӧ�ù�ס
                    int[] fourthCards = mainForm.currentPokers[users[2] - 1].GetSuitCards(firstSuit);
                    if (fourthCards.Length>0)
                    {
                        int fourthMax = fourthCards[fourthCards.Length -1];
                        if (!CommonMethods.CompareTo(firstMax, fourthMax, suit, rank, firstSuit))
                        {
                            
                            //���ļ������Ӧ�ó����ķǷ���
                            //������бȵ��ļҴ����
                            if (CommonMethods.CompareTo(myMax, fourthMax, suit, rank, firstSuit))
                            {
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, myMax);
                            }
                            else //��Ҳ�ܲ�ס
                            {
                                SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, true);
                                SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, true);

                                SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                                SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                            }

                        }
                        else
                        {
                            SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList,true);
                            SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                        }
                    }
                    else
                    {
                        SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList,true); //�����ƻ�����С����
                        SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList,false); //�����ƻ�����С����
                    }

                    if ((!CommonMethods.CompareTo(firstMax, (int)sendedCards[0], suit, rank, firstSuit)) && (!CommonMethods.CompareTo(max2, (int)sendedCards[0], suit, rank, firstSuit)))
                    {
                        mainForm.whoIsBigger = whoseOrder;
                    }

                }
                else if (!CommonMethods.CompareTo(max2, myMax, suit, rank, firstSuit)) //�׼���С���ҵ����
                {
                    //������
                    if (myMax > -1)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myMax);
                        mainForm.whoIsBigger = whoseOrder;


                        return;
                    }
                }



                SendThisSuitNoScores(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);

                SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                return;

            }
            else if ((pairTotal == 1) && (count == 2)) //�׼ҳ���һ����
            {
                ArrayList list = mySuitCP.GetPairs();
                if (list.Count >= 1 && (secondCP.GetPairs().Count < 1)) //�����жԣ��ڶ����޶�
                {
                    if (!CommonMethods.CompareTo((int)mainForm.currentSendCards[mainForm.firstSend-1][0],(int)list[0],suit,rank,firstSuit))
                    {
                        mainForm.whoIsBigger = whoseOrder;
                    }
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);

                   
                    return;
                    
                }
                else if (list.Count >= 1 && (secondCP.GetPairs().Count >= 1)) //���Ƕ��ж�
                {
                    int myMax = (int)list[list.Count - 1];
                   
                    int max2 = (int)secondCP.GetPairs()[0];

                    //����ҵĵ��ƴ��ڵڶ��ҵ���
                    if (!CommonMethods.CompareTo(max2, myMax, suit, rank,firstSuit))
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[list.Count - 1]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[list.Count - 1]);
                        if (!CommonMethods.CompareTo((int)mainForm.currentSendCards[mainForm.firstSend - 1][0], (int)list[0], suit, rank, firstSuit))
                        {
                            mainForm.whoIsBigger = whoseOrder;
                        }
                        
                        return;
                    }
                    else //����
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);

                     
                        return;
                    }
                }
                else if (list.Count < 1 && secondCP.GetPairs().Count >= 1) //����ڶ���Ҳ���˶�,���޶�
                {
                    int max2 = (int)secondCP.GetPairs()[0];
                    //�׼Ҵ�
                    if (CommonMethods.CompareTo(firstMax, max2, suit, rank, firstSuit))
                    {
                        SendThisSuitOrScores(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                        SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                        SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                        SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                        
                        return;
                    }
                    else
                    {
                        SendThisSuitNoScores(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                        SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                        SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                        SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                       
                        return;
                    }
                }
                else if (list.Count < 1 && secondCP.GetPairs().Count < 1)
                {
                    //Ŀǰֻ�жԼҳ��˶�
                    ArrayList fourthPairs = mainForm.currentPokers[users[2] - 1].GetPairs(firstSuit);
                    if (fourthPairs.Count > 0)
                    {
                        int fourthMax = (int)fourthPairs[fourthPairs.Count-1];
                        if (!CommonMethods.CompareTo(firstMax, fourthMax, suit, rank, firstSuit))
                        {
                            //���ļ������Ӧ�ó����ķǷ���
                            SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList,true);
                            SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                        }
                        else
                        {
                            SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList,true);
                            SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                        }
                    }
                    else
                    {
                        SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList,true); //�����ƻ�����С����
                        SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                    }


                    SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                    SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                   
                    return;
                }

            }
            else if (count == pairTotal * 2 && count > 0) //����ԣ��϶��׼����
            {
                ArrayList list = mySuitCP.GetPairs();
                for (int i = 0; i < pairTotal && i < list.Count; i++)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[i]);
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[i]);
                }
                SendThisSuitOrScores(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                return;
            }
            else //�����˦��
            {
                ArrayList list = mySuitCP.GetPairs();
                for (int i = 0; i < pairTotal && i < list.Count; i++)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[i]);
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[i]);
                }

                //�������С����
                SendThisSuitOrScores(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

              
                return;
            }


           

        }
        //whoseOrder�ǵ��ĸ�����
        internal static void WhoseOrderIs4(MainForm mainForm, CurrentPoker[] currentPokers, int whoseOrder, ArrayList sendedCards, int count, int suit, int rank, int firstSuit)
        {
            ArrayList firstSendCards = mainForm.currentSendCards[mainForm.firstSend - 1]; //�׼ҳ�����
            CurrentPoker firstCP = new CurrentPoker();
            firstCP.Suit = suit;
            firstCP.Rank = rank;
            firstCP = CommonMethods.parse(firstSendCards, suit, rank); //�׼ҳ�����

            int firstMax = CommonMethods.GetMaxCard(firstSendCards, suit, rank); //�õ��׼ҳ���������
            int pairTotal = firstCP.GetPairs().Count;

            CurrentPoker myCP = currentPokers[whoseOrder - 1];         //�ҵ���
            ArrayList myPokerList = mainForm.pokerList[whoseOrder - 1]; //�ҵ���


            //whose�Ĵ˻�ɫ������
            int myTotal = CommonMethods.GetSuitCount(currentPokers[whoseOrder - 1], suit, rank, firstSuit); //�˻�ɫ����
            //�˻�ɫ����
            int[] cards = myCP.GetSuitCards(firstSuit); //�˻�ɫ����
           
           

            ArrayList myList = new ArrayList(cards);
            CurrentPoker mySuitCP = new CurrentPoker();  //�ҵĴ˻�ɫ����
            mySuitCP.Suit = suit;
            mySuitCP.Rank = rank;
            mySuitCP = CommonMethods.parse(myList, suit, rank);
            mySuitCP.Sort();

            firstCP.Sort();
            myCP.Sort();

            int[] users = CommonMethods.OtherUsers(mainForm.firstSend); //������λ�û�

            CurrentPoker secondCP = new CurrentPoker();
            secondCP.Suit = suit;
            secondCP.Rank = rank;
            secondCP = CommonMethods.parse(mainForm.currentSendCards[users[0] - 1], suit, rank); //�׼Һ���һ���û�

            CurrentPoker thirdCP = new CurrentPoker();
            thirdCP.Suit = suit;
            thirdCP.Rank = rank;
            thirdCP = CommonMethods.parse(mainForm.currentSendCards[users[1] - 1], suit, rank); //�׼Һ�ڶ����û�

            int[] tmpUsers = CommonMethods.OtherUsers(whoseOrder);
            
            //���Ǳ�
            if (myTotal == 0)
            {
                if (firstSuit != suit)
                {
                    //���Ŀǰ������һ������ 
                    int biggerMax = (int)mainForm.currentSendCards[mainForm.whoIsBigger - 1][0];
                    

                    if (mainForm.whoIsBigger == tmpUsers[1])
                    {
                        //���ϣ������п������ĸ���Ҳ�ȴ����һ�Ҵ�
                        SendOtherSuitOrScores(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ�Ƿ���
                        SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ����
                        SendOtherSuitOrScores(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ�Ƿ���
                        SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ����


                        int sendOtherSuitsTotal = sendedCards.Count; //û�и��ƿ�����ֻ�ܳ���

                        if (firstCP.HasTractors() && sendOtherSuitsTotal == 0) //������
                        {
                            int minMaster = myCP.GetMasterTractor();
                            int tmpFirstTractor = firstCP.GetTractor();

                            //����ҵ����ܴ�������Ǽҵ���

                            if ((!CommonMethods.CompareTo(tmpFirstTractor, minMaster, suit, rank, firstSuit)) && (minMaster> -1))
                            {
                                mainForm.whoIsBigger = whoseOrder;
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, minMaster);
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, minMaster);
                                int[] ttt = myCP.GetTractorOtherCards(minMaster);
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, ttt[1]);
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, ttt[1]);
                            }
                        }
                        else if (myCP.GetMasterCardsTotal() >= count && count == 1 && sendOtherSuitsTotal== 0) //������
                        {
                            int minMaster = myCP.GetMinMasterCards(suit);
                            //����ҵ����ܴ�������Ǽҵ���
                            if (!CommonMethods.CompareTo(biggerMax, minMaster, suit, rank, firstSuit))
                            {
                                mainForm.whoIsBigger = whoseOrder;
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, minMaster);
                                return;
                            }
                        }
                        else if (myCP.GetMasterCardsTotal() >= count &&  pairTotal == 1 && count == 2 && sendOtherSuitsTotal == 0) //��һ����ʱ
                        {
                            ArrayList masterPairs = myCP.GetMasterPairs();
                            //����ҵ����ܴ�������Ǽҵ���
                            if (masterPairs.Count > 0)
                            {
                                mainForm.whoIsBigger = whoseOrder;
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)masterPairs[0]);
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)masterPairs[0]);
                                return;
                            }
                        }
                        else if (myCP.GetMasterCardsTotal() >= count &&  pairTotal == 0 && count > 1 && sendOtherSuitsTotal == 0) //����˦��
                        {
                            int minMaster = myCP.GetMinMasterCards(suit);
                            //����ҵ����ܴ�������Ǽҵ���
                            if (!CommonMethods.CompareTo(biggerMax, minMaster, suit, rank, firstSuit))
                            {
                                mainForm.whoIsBigger = whoseOrder;
                                CommonMethods.SendCards(sendedCards, myCP, myPokerList, minMaster);

                                SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, true);
                                SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, true);
                                SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, false);
                                SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, false);

                                return;
                            }

                        }

                        SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, true); //���Ƿ���
                        SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, true); //������
                        SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, false); //���Ƿ���
                        SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, false); //������

                       
                    }
                    else
                    {
                        if (firstCP.HasTractors()) //��һ����ʱ
                        {
                            SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ�Ƿ���
                            SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ����
                            SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ�Ƿ���
                            SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ����
                        }
                        else if (myCP.GetMasterCardsTotal() >= count && count == 1) //������
                        {

                            //int maxMaster = myCP.GetMaxMasterCards();
                            int[] masterCards = myCP.GetSuitCards(suit);
                            for (int i = 0; i < masterCards.Length; i++)
                            {
                                //����ҵ����ܴ�������Ǽҵ���
                                if (!CommonMethods.CompareTo(biggerMax, masterCards[i], suit, rank, firstSuit))
                                {
                                    mainForm.whoIsBigger = whoseOrder;
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, masterCards[i]);
                                    return;
                                }
                            }

                            SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ�Ƿ���
                            SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ�Ƿ���
                            SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ����
                            SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ����

                        }
                        else if (myCP.GetMasterCardsTotal() >= count &&  pairTotal == 1 && count == 2) //��һ����ʱ
                        {
                            ArrayList masterPairs = myCP.GetMasterPairs();
                            //����ҵ����ܴ�������Ǽҵ���
                            

                            if (masterPairs.Count > 0)
                            {
                                for (int i = 0; i < masterPairs.Count; i++)
                                {
                 
                                    if (!CommonMethods.CompareTo(biggerMax, (int)masterPairs[i], suit, rank, firstSuit))
                                    {
                                        mainForm.whoIsBigger = whoseOrder;
                                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)masterPairs[0]);
                                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)masterPairs[0]);
                                        return;
                                    }
                                   
                                }
                            }
                        }
                        else if (myCP.GetMasterCardsTotal() >= count &&  pairTotal == 0 && count > 1) //����˦��
                        {
                            int maxMaster = myCP.GetMaxMasterCards();
                            //����ҵ����ܴ�������Ǽҵ���
                            int[] masterCards = myCP.GetSuitCards(suit);
                            for (int i = 0; i < masterCards.Length; i++)
                            {
                                if (!CommonMethods.CompareTo(biggerMax, masterCards[i], suit, rank, firstSuit))
                                {
                                    mainForm.whoIsBigger = whoseOrder;
                                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, masterCards[i]);

                                    SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, true);
                                    SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, true);
                                    SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, false);
                                    SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, false);

                                    return;
                                }
                            }

                        }
                    }
                }
            }
            
            if (myTotal < count) //����ɫ�����Ͳ���
            {

                for (int i = 0; i < myTotal; i++) //�Ƚ��˻�ɫ
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList,cards[i]);
                }

                if (mainForm.whoIsBigger == tmpUsers[1])
                {
                    SendOtherSuitOrScores(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ�Ƿ���
                    SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ����
                    SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, true); //���Ƿ���
                    SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, true); //������

                    SendOtherSuitOrScores(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ�Ƿ���
                    SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ����
                    SendMasterSuitOrScores(sendedCards, count, suit, myCP, myPokerList, false); //���Ƿ���
                    SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, false); //������
                }
                else
                {
                    SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ�Ƿ���
                    SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, true); //������ɫ����
                    SendMasterSuitNoScores(sendedCards, count, suit, myCP, myPokerList, true); //���Ƿ���
                    SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, true); //������

                    SendOtherSuitNoScores(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ�Ƿ���
                    SendOtherSuit(sendedCards, count, firstSuit, myCP, myPokerList, false); //������ɫ����
                    SendMasterSuitNoScores(sendedCards, count, suit, myCP, myPokerList, false); //���Ƿ���
                    SendMasterSuit(sendedCards, count, suit, myCP, myPokerList, false); //������
                }

             
                return;
            }
           //�����Ǵ˻�ɫ���Ʊ��׼ҳ����ƶ�
            else if (firstCP.HasTractors())  //����׼ҳ���������
            {
                //�����������������������������ʣ������������
                if (mySuitCP.HasTractors())
                {
                    int k = mySuitCP.GetTractor();
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, k);
                    int[] ks = mySuitCP.GetTractorOtherCards(k);
                    for (int i = 0; i < 3; i++)
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, ks[i]);
                    }


                    CurrentPoker tmpCP = CommonMethods.parse(mainForm.currentSendCards[mainForm.whoIsBigger - 1], suit, rank);
                    int tmp = tmpCP.GetTractor();
                    if (!CommonMethods.CompareTo(tmp, k, suit, rank, firstSuit))
                    {
                        mainForm.whoIsBigger = whoseOrder;
                    }
                    
                }
                else if (mySuitCP.GetPairs().Count > 0) //����ж�
                {
                    ArrayList list = mySuitCP.GetPairs();
                    if (list.Count >= 2) //������ж����,�����ٳ�������
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[1]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[1]);
                       
                    }
                    else //����ֻ�ܳ�һ����,�����С��
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                    }


                }


                //��Ȼ�󲻹��׼ң�����С����
                //���ң�����ɫ���Կ����������
                if (mainForm.whoIsBigger == tmpUsers[1])
                {
                    SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, true); //�˻�ɫ�ķǷֵ���
                    SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, true); //�˻�ɫ�ķ���

                    SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false); //�˻�ɫ�ķǷֵ���
                    SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false); //�˻�ɫ�ķ���
                }
                else
                {
                    SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, true); //�˻�ɫ�ķǷֵ���
                    SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, true); //�˻�ɫ�ķ���

                    SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false); //�˻�ɫ�ķǷֵ���
                    SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false); //�˻�ɫ�ķ���
                }


                return;
            }
            else if (count == 1) //������,������Ŷȷʵ�д˻�ɫ����
            {
                int myMax = -1; //�ҵĴ˻�ɫ�����
                if (firstSuit == suit)
                {
                    myMax = mySuitCP.GetMaxMasterCards();
                }
                else
                {
                    myMax = mySuitCP.GetMaxCards(firstSuit);
                }


                int max2 = CommonMethods.GetMaxCard(mainForm.currentSendCards[users[0] - 1], suit, rank); //�ڶ���
                int max3 = CommonMethods.GetMaxCard(mainForm.currentSendCards[users[1] - 1], suit, rank); //������

               
                //�Լ�(�ڶ���)��
                if ((!CommonMethods.CompareTo(firstMax, max2, suit, rank,firstSuit)) && (CommonMethods.CompareTo(max2,max3,suit,rank,firstSuit)))
                {
                    SendThisSuitOrScores(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                    SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                    if (!CommonMethods.CompareTo(max2,(int)sendedCards[0],suit,rank,firstSuit))
                    {
                        mainForm.whoIsBigger = whoseOrder;
                    }
                   
                    return;
                } //�Ҵ�
                else if ((!CommonMethods.CompareTo(firstMax, myMax, suit, rank, firstSuit)) && (!CommonMethods.CompareTo(max3, myMax, suit, rank, firstSuit)))
                {
                    if (myMax > -1) //����Ӧ����ԶΪtrue
                    {
                        CommonMethods.SendCards(sendedCards, myCP, myPokerList, myMax);
                        mainForm.whoIsBigger = whoseOrder;

                        return;
                    }
                }

                SendThisSuitNoScores(sendedCards, count, suit,firstSuit, myCP, myPokerList,true); //���ǲ��󣬳�С�Ƿ���
                SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true); //������

                SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false); //���ǲ��󣬳�С�Ƿ���
                SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false); //������

                if ((!CommonMethods.CompareTo(firstMax, (int)sendedCards[0], suit, rank, firstSuit)) && (!CommonMethods.CompareTo(max2, (int)sendedCards[0], suit, rank, firstSuit)) && (!CommonMethods.CompareTo(max3, myMax, suit, rank, firstSuit)))
                {
                    mainForm.whoIsBigger = whoseOrder;
                }

                return;
            }
            else if ((pairTotal == 1) && (count == 2)) //�����һ����
            {
                ArrayList list = mySuitCP.GetPairs(); //�ҵĶ�
                //����ҶԼҴ�
                bool b2 = secondCP.GetPairs().Count > 0; //����Լ��ж�
                bool b3 = thirdCP.GetPairs().Count > 0; //���������Ҳ���˶�

                int max2 = -1;
                int max3 = -1;

                if (b2)
                {
                    max2 = (int)secondCP.GetPairs()[0];
                }
                if (b3)
                {
                    max3 = (int)thirdCP.GetPairs()[0];
                }

                //������ж�
                if (list.Count > 0)
                {
                    int myMax = (int)list[list.Count - 1];
                   
                    if (b2 && b3) //2,3���ж�
                    {
                        //�ԼҴ�
                        if ((!CommonMethods.CompareTo(firstMax, max2, suit, rank, firstSuit)) && (CommonMethods.CompareTo(max2, max3, suit, rank, firstSuit)))
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);

                            if (!CommonMethods.CompareTo(max2, (int)list[0], suit, rank, firstSuit))
                            {
                                mainForm.whoIsBigger = whoseOrder;
                            }

                         
                            return;
                        }//����Ҵ�
                        else if ((!CommonMethods.CompareTo(firstMax, myMax, suit, rank, firstSuit)) && (!CommonMethods.CompareTo(max3, myMax, suit, rank, firstSuit)))
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[list.Count - 1]);
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[list.Count - 1]);
                            mainForm.whoIsBigger = whoseOrder;
                            
                            return;
                        }
                        else //�Է���
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);

                            return;
                        }

                       

                    }
                    else if (b2 && (!b3)) //2�ж�,3�޶�
                    {
                        //�ԼҴ�
                        if ((!CommonMethods.CompareTo(firstMax, max2, suit, rank, firstSuit)))
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                            if (!CommonMethods.CompareTo(max2, (int)list[0], suit, rank, firstSuit))
                            {
                                mainForm.whoIsBigger = whoseOrder;
                            }


                        
                            return;
                        } //�Ҵ�
                        else if ((!CommonMethods.CompareTo(firstMax, myMax, suit, rank, firstSuit)))
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[list.Count - 1]);
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[list.Count - 1]);

                            mainForm.whoIsBigger = whoseOrder;
                            return;
                        }
                        else
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);

                            return;
                        }
                    }
                    else if ((!b2) && b3) //2�޶ԣ�3�ж�
                    {
                        //����Ҵ�
                        if ((!CommonMethods.CompareTo(firstMax, myMax, suit, rank, firstSuit)) && (!CommonMethods.CompareTo(max3, myMax, suit, rank, firstSuit)))
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[list.Count - 1]);
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[list.Count - 1]);
                            mainForm.whoIsBigger = whoseOrder;

                            return;
                        }
                        else
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                           
                            return;
                        }
                    }
                    else if ((!b2) && (!b3)) //2,3���޶�
                    {
                        if ((!CommonMethods.CompareTo(firstMax, myMax, suit, rank, firstSuit)))
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[list.Count - 1]);
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[list.Count - 1]);
                            mainForm.whoIsBigger = whoseOrder;
                            

                            return;
                        }
                        else
                        {
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                            CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[0]);
                            if (!CommonMethods.CompareTo(firstMax, (int)list[0], suit, rank, firstSuit))
                            {
                                mainForm.whoIsBigger = whoseOrder;
                            }

                            return;
                        }
                    }
                }
                else //������޶�
                {
                    if (b2 && b3) //2,3���ж�
                    {
                        //�ԼҴ�
                        if ((!CommonMethods.CompareTo(firstMax, max2, suit, rank, firstSuit)) && (CommonMethods.CompareTo(max2, max3, suit, rank, firstSuit)))
                        {
                            SendThisSuitOrScores(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                            SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                            return;
                        }
                        else
                        {
                            SendThisSuitNoScores(sendedCards, count, suit,firstSuit, myCP, myPokerList,true);
                            SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                            SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                            SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                            return;
                        }
                        
                    }
                    else if (b2 && (!b3))
                    {
                        //�ԼҴ�
                        if ((!CommonMethods.CompareTo(firstMax, max2, suit, rank, firstSuit)))
                        {
                            SendThisSuitOrScores(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                            SendThisSuitOrScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                            return;
                        }
                        else
                        {
                            SendThisSuitNoScores(sendedCards, count, suit,firstSuit, myCP, myPokerList,true);
                            SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                            SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                            SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                            return;
                        }
                    }
                    else
                    {
                        SendThisSuitNoScores(sendedCards, count, suit,firstSuit, myCP, myPokerList,true);
                        SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                        SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                        SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);

                        return;
                    }
                    
                }

            }
            else if (count == pairTotal * 2 && (count > 0)) //���Ƕ�,�϶����������
            {
                ArrayList list = mySuitCP.GetPairs();
                for (int i = 0; i < pairTotal && i < list.Count; i++)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[i]);
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[i]);
                }

                //�������С����
                SendThisSuitNoScores(sendedCards, count, suit,firstSuit, myCP, myPokerList,true);
                SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
              
                return;
            }
            else //�жԺ��е����ƣ���˦��
            {
                ArrayList list = mySuitCP.GetPairs();
                for (int i = 0; i < pairTotal && i < list.Count; i++)
                {
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[i]);
                    CommonMethods.SendCards(sendedCards, myCP, myPokerList, (int)list[i]);
                }

                //�������С���ƣ�˳�򣬴˻�ɫ�Ƿ��ƣ��˻�ɫ����
                SendThisSuitNoScores(sendedCards, count, suit,firstSuit, myCP, myPokerList,true);
                SendThisSuit(sendedCards, count,suit, firstSuit, myCP, myPokerList,true);
                SendThisSuitNoScores(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
                SendThisSuit(sendedCards, count, suit, firstSuit, myCP, myPokerList, false);
               
                return;
            }
           
        }

        
    }
}
