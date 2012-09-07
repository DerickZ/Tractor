using System;
using System.Collections;
using System.Text;

namespace Kuaff.Tractor
{
    /// <summary>
    /// ���ȳ��Ƶ��㷨
    /// </summary>
    class ShouldSendedCardsAlgorithm
    {
        internal static void ShouldSendCards(MainForm mainForm, CurrentPoker[] currentPokers, int whoseOrder, ArrayList sendedCards)
        {
            currentPokers[0].Sort();
            currentPokers[1].Sort();
            currentPokers[2].Sort();
            currentPokers[3].Sort();

            mainForm.whoIsBigger = whoseOrder;

            //1.�������ĸ��Ƴ�
            CurrentPoker cp = currentPokers[whoseOrder - 1];

           

            #region ���������������
            int t = cp.GetNoRankNoSuitTractor(); //����������
            if (t > -1)
            {
                int[] othercards = cp.GetTractorOtherCards(t);

                CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], t);
                CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], t);
                CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], othercards[1]);
                CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], othercards[1]);


                return;
            }
            #endregion // ���������������

           


            #region ����и������Ķ�
            if (cp.GetNoRankNoSuitPairs().Count > 0 )
            {
                

                ArrayList al = cp.GetNoRankNoSuitPairs();
                int[] max = { (int)al[al.Count - 1], (int)al[al.Count - 1] };

                bool b3 = cp.Count < 25;
                bool b1 = cp.Rank != 12 && (((int)al[al.Count - 1] % 13) == 12) ;
                bool b2 = cp.Rank == 12 && (((int)al[al.Count - 1] % 13) == 11) ;

                if (b3 || b1 || b2) //����Aʱ
                {

                    if (whoseOrder == 2)
                    {
                        if ((!currentPokers[2].CompareTo(max)) && (!currentPokers[3].CompareTo(max)))
                        {
                          
                            CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], max[0]);
                            CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], max[0]);


                            if ((mainForm.currentRank != 12) && (mainForm.currentRank != 11)) //ͬʱ˦A
                            {
                                if ((max[0] % 13) == 11)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 12) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 12);
                                    }
                                }
                                else if ((max[0] % 13) == 12)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 11) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 11);
                                    }
                                }
                            }
                            else if ((mainForm.currentRank != 12) && (mainForm.currentRank == 11)) //ͬʱ˦A
                            {
                                if ((max[0] % 13) == 10)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 12) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 12);
                                    }
                                }
                                else if ((max[0] % 13) == 12)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 10) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 10);
                                    }
                                }
                            }
                            else if (mainForm.currentRank == 12) //ͬʱ˦A
                            {
                                if ((max[0] % 13) == 10)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 11) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 11);
                                    }
                                }
                                else if ((max[0] % 13) == 11)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 10) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 10);
                                    }
                                }
                            }

                           

                            return;
                        }

                    }
                    else if (whoseOrder == 3)
                    {
                        if ((!currentPokers[0].CompareTo(max)) && (!currentPokers[1].CompareTo(max)))
                        {
                           

                            CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], max[0]);
                            CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], max[0]);

                            if ((mainForm.currentRank != 12) && (mainForm.currentRank != 11)) //ͬʱ˦A
                            {
                                if ((max[0] % 13) == 11)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 12) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 12);
                                    }
                                }
                                else if ((max[0] % 13) == 12)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 11) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 11);
                                    }
                                }
                            }
                            else if ((mainForm.currentRank != 12) && (mainForm.currentRank == 11)) //ͬʱ˦A
                            {
                                if ((max[0] % 13) == 10)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 12) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 12);
                                    }
                                }
                                else if ((max[0] % 13) == 12)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 10) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 10);
                                    }
                                }
                            }
                            else if (mainForm.currentRank == 12) //ͬʱ˦A
                            {
                                if ((max[0] % 13) == 10)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 11) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 11);
                                    }
                                }
                                else if ((max[0] % 13) == 11)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 10) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 10);
                                    }
                                }
                            }

                           
                            return;
                        }
                    }
                    else if (whoseOrder == 4)
                    {
                        if ((!currentPokers[0].CompareTo(max)) && (!currentPokers[1].CompareTo(max)))
                        {
                           

                            CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], max[0]);
                            CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], max[0]);

                            if ((mainForm.currentRank != 12) && (mainForm.currentRank != 11)) //ͬʱ˦A
                            {
                                if ((max[0] % 13) == 11)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 12) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 12);
                                    }
                                }
                                else if ((max[0] % 13) == 12)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 11) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 11);
                                    }
                                }
                            }
                            else if ((mainForm.currentRank != 12) && (mainForm.currentRank == 11)) //ͬʱ˦A
                            {
                                if ((max[0] % 13) == 10)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 12) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 12);
                                    }
                                }
                                else if ((max[0] % 13) == 12)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 10) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 10);
                                    }
                                }
                            }
                            else if (mainForm.currentRank == 12) //ͬʱ˦A
                            {
                                if ((max[0] % 13) == 10)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 11) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 11);
                                    }
                                }
                                else if ((max[0] % 13) == 11)
                                {
                                    if (cp.GetCardCount((CommonMethods.GetSuit(max[0]) - 1) * 13 + 10) == 1)
                                    {
                                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (CommonMethods.GetSuit(max[0]) - 1) * 13 + 10);
                                    }
                                }
                            }

                           

                            return;
                        }
                    }
                }
            }
            #endregion // ����и������Ķ�

           

            #region ����е���������
            //�жϵ�����
            int maxCards = -1;
            for (int i = 1; i < 5; i++)
            {
                if (i == cp.Suit)
                {
                    continue;
                }

                maxCards = cp.GetMaxCards(i);
                if (maxCards == -1)
                {
                    continue;
                }

                if (whoseOrder == 2)
                {
                    if ((!currentPokers[2].CompareTo(maxCards)) && (!currentPokers[3].CompareTo(maxCards)))
                    {
                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], maxCards);

                       
                        return;
                    }

                }
                else if (whoseOrder == 3)
                {
                    if ((!currentPokers[0].CompareTo(maxCards)) && (!currentPokers[1].CompareTo(maxCards)))
                    {
                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], maxCards);

                      
                        return;
                    }
                }
                else if (whoseOrder == 4)
                {
                    if ((!currentPokers[0].CompareTo(maxCards)) && (!currentPokers[1].CompareTo(maxCards)))
                    {
                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], maxCards);

                        return;
                    }
                }
            }
            #endregion // ����е���������

          


            #region ��Ȼû�������,���Լ��Ƿ������ĸ���
            for (int i = 1; i < 5; i++)
            {
                if (i == cp.Suit)
                {
                    continue;
                }

                if (whoseOrder == 2)
                {
                    maxCards = currentPokers[0].GetMaxCards(i);
                    if (maxCards == -1)
                    {
                        continue;
                    }

                    if ((!currentPokers[2].CompareTo(maxCards)) && (!currentPokers[3].CompareTo(maxCards)))
                    {
                        int max2 = currentPokers[2].GetMaxCard(i);
                        int max3 = currentPokers[2].GetMaxCard(i);

                        if (CommonMethods.CompareTo(max2, max3, cp.Suit, cp.Rank, i))
                        {
                            int rt = currentPokers[whoseOrder - 1].GetMinCardsOrScores(i);
                            if (rt > -1)
                            {
                                CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], rt);
                                return;
                            }
                        }
                    }

                }
                else if (whoseOrder == 3)
                {
                    maxCards = currentPokers[3].GetMaxCards(i);
                    if (maxCards == -1)
                    {
                        continue;
                    }
                    if ((!currentPokers[0].CompareTo(maxCards)) && (!currentPokers[1].CompareTo(maxCards)))
                    {
                        int max2 = currentPokers[2].GetMaxCard(i);
                        int max3 = currentPokers[2].GetMaxCard(i);

                        if (CommonMethods.CompareTo(max2, max3, cp.Suit, cp.Rank, i))
                        {
                            int rt = currentPokers[whoseOrder - 1].GetMinCardsOrScores(i);
                            if (rt > -1)
                            {
                                CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], rt);
                                return;
                            }
                        }
                    }
                }
                else if (whoseOrder == 4)
                {
                    maxCards = currentPokers[2].GetMaxCards(i);
                    if (maxCards == -1)
                    {
                        continue;
                    }

                    if ((!currentPokers[0].CompareTo(maxCards)) && (!currentPokers[1].CompareTo(maxCards)))
                    {
                         int max2 = currentPokers[2].GetMaxCard(i);
                        int max3 = currentPokers[2].GetMaxCard(i);

                        if (CommonMethods.CompareTo(max2, max3, cp.Suit, cp.Rank, i))
                        {
                            int rt = currentPokers[whoseOrder - 1].GetMinCardsOrScores(i);
                            if (rt > -1)
                            {
                                CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], rt);
                                return;
                            }
                        }
                    }
                }
            }
            #endregion // ��Ȼû�������,���Լ��Ƿ������ĸ���

           

            //3.�Ƿ�������

            #region ���û�и��ƿɳ�������
            if (currentPokers[whoseOrder - 1].GetMasterCardsTotal() > 0)
            {
                t = currentPokers[whoseOrder - 1].GetMasterTractor();
                if (t > -1 && t != 53)
                {
                    int[] ttt = currentPokers[whoseOrder - 1].GetTractorOtherCards(t);
                    CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], t);
                    CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], t);
                    CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], ttt[1]);
                    CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], ttt[1]);

                  
                    return;
                }

                //��
                ArrayList al = currentPokers[whoseOrder - 1].GetMasterPairs();

                int[] users = CommonMethods.OtherUsers(whoseOrder);

                if (currentPokers[whoseOrder - 1].Count<= 12 && al.Count>0)
                {
                    int t1 = currentPokers[whoseOrder - 1].GetMasterCardsTotal();
                    int t2 = currentPokers[users[0] - 1].GetMasterCardsTotal();
                    int t3 = currentPokers[users[2] - 1].GetMasterCardsTotal();

                    if (t1 > t2 && t1 > t3 )
                    {
                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (int)al[al.Count - 1]);
                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (int)al[al.Count - 1]);
                        return;
                    }
                   
                    
                }

                int rt = currentPokers[whoseOrder - 1].GetMinCardsNoScores(cp.Suit);
                if (rt > -1 && currentPokers[whoseOrder - 1].GetCardCount(rt) == 1)
                {
                    CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], rt);
                    
                    return;
                }
                //
                if (cp.Suit != 1)
                {
                    if (cp.HeartsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], cp.Rank);
                      
                        return;
                    }
                }
                if (cp.Suit != 2)
                {
                    if (cp.PeachsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], cp.Rank + 13);
                   
                        return;
                    }
                }
                if (cp.Suit != 3)
                {
                    if (cp.DiamondsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], cp.Rank + 26);
                     
                        return;
                    }
                }
                if (cp.Suit != 4)
                {
                    if (cp.ClubsRankTotal == 1)
                    {
                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], cp.Rank + 39);
                    
                        return;
                    }
                }

            }
            
            //�����Ķ�

            #endregion // ���û�и��ƿɳ�������

           
            //5.����С�ĸ���
            #region ���ܼ��������Ļ�������һ�Ų��Ƿֵĸ���
            for (int i = 0; i < 5; i++)
            {
                if (i == cp.Suit)
                {
                    continue;
                }
                int rt = currentPokers[whoseOrder - 1].GetMinCardsNoScores(i);
                if (rt != 3 && rt != 8 && rt != 11)
                {

                    if (rt > -1)
                    {
                        CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], rt);

                        return;
                    }
                }
            }
            #endregion // ���ܼ��������Ļ�������һ�Ų��Ƿֵĸ���


            

            //6.ȫʣ���ƺͷָ��ƣ�����
            #region ȫʣ���ƺͷָ���
            ArrayList mPairs = currentPokers[whoseOrder - 1].GetMasterPairs();
            if (mPairs.Count > 0) //�ȳ�����
            {
                int rt = (int)mPairs[mPairs.Count - 1];
                CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], rt);
                CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], rt);
               
                return;
            }
            int maserMin = currentPokers[whoseOrder - 1].GetMinMasterCards(cp.Suit);
            //if ((maserMin > -1) && (maserMin % 13 = 3) && (maserMin % 13 = 8) && (maserMin % 13 = 11))
            if (maserMin > -1)
            {
                CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], maserMin);
               
                return;
            }

            for (int i = 1; i < 5; i++)
            {
                int rt = currentPokers[whoseOrder - 1].GetMinCards(i);
                if (rt > -1)
                {
                    CommonMethods.SendCards(sendedCards, currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], rt);
                   
                    return;
                }
            }
            #endregion // ȫʣ���ƺͷָ���

           
        }



    }
}
