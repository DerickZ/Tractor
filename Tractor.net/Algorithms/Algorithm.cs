using System;
using System.Collections;
using System.Text;

using Kuaff.Tractor.Plugins;

namespace Kuaff.Tractor
{
    /// <summary>
    /// �㷨��.
    /// Ϊ������û��ṩ�㷨֧��
    /// </summary>
    class Algorithm
    {

        #region �����㷨
        //�Ƿ��������
        internal static int ShouldSetRank(MainForm mainForm, int user)
        {
            CurrentPoker currentPoker = mainForm.currentPokers[user - 1];

            int rank = currentPoker.Rank;
            if (rank == 0 || rank == 0 || rank == 8 || rank == 11)
            {
                if (currentPoker.Clubs[rank] > 0)
                {
                    return 4;
                }
                else if (currentPoker.Diamonds[rank] > 0)
                {
                    return 3;
                }
                else if (currentPoker.Peachs[rank] > 0)
                {
                    return 2;
                }
                else if (currentPoker.Hearts[rank] > 0)
                {
                    return 1;
                }
            }

            //�����û��������Ȼ���ܷ��ƣ�ֻ��������

            int count = 0;
            int user2 = 0;
            if (currentPoker.Count == 23) //���������23���ƣ����ڶԼҵ�������
            {

                if (user == 2)
                {
                    user2 = 1;
                }
                else if (user == 3)
                {
                    user2 = 4;
                }
                else if (user == 4)
                {
                    user2 = 3;
                }
                else if (user == 1)
                {
                    user2 = 2;
                }
            }
            else
            {
                user2 = user;
            }

            //
            count = (mainForm.currentPokers[user2 - 1].ClubsNoRankTotal + mainForm.currentPokers[user2 - 1].DiamondsNoRankTotal + mainForm.currentPokers[user2 - 1].PeachsNoRankTotal + mainForm.currentPokers[user2 - 1].HeartsNoRankTotal) / 4;

            if (currentPoker.Clubs[rank] > 0 && (mainForm.currentPokers[user2 - 1].Clubs[rank] > count))
            {
                return 4;
            }
            else if (currentPoker.Diamonds[rank] > 0 && (mainForm.currentPokers[user2 - 1].Diamonds[rank] > count))
            {
                return 3;
            }
            else if (currentPoker.Peachs[rank] > 0 && (mainForm.currentPokers[user2 - 1].Peachs[rank] > count))
            {
                return 2;
            }
            else if (currentPoker.Hearts[rank] > 0 && (mainForm.currentPokers[user2 - 1].Hearts[rank] > count))
            {
                return 1;
            }


            return 0;
        }
        //Again
        internal static int ShouldSetRankAgain(MainForm mainForm, CurrentPoker currentPoker)
        {
            if (!(mainForm.showSuits == 1))
            {
                return 0;
            }


            int rank = currentPoker.Rank;

            if (rank == 0)
            {
                if (currentPoker.Clubs[rank] > 1)
                {
                    return 4;
                }
                else if (currentPoker.Diamonds[rank] > 1)
                {
                    return 3;
                }
                else if (currentPoker.Peachs[rank] > 1)
                {
                    return 2;
                }
                else if (currentPoker.Hearts[rank] > 1)
                {
                    return 1;
                }
            }

            int count = (currentPoker.ClubsNoRankTotal + currentPoker.DiamondsNoRankTotal + currentPoker.PeachsNoRankTotal + currentPoker.HeartsNoRankTotal) / 4;
            if (currentPoker.Clubs[rank] > 1 && (currentPoker.Clubs[rank] > count))
            {
                return 4;
            }
            else if (currentPoker.Diamonds[rank] > 1 && (currentPoker.Diamonds[rank] > count))
            {
                return 3;
            }
            else if (currentPoker.Peachs[rank] > 1 && (currentPoker.Peachs[rank] > count))
            {
                return 2;
            }
            else if (currentPoker.Hearts[rank] > 1 && (currentPoker.Hearts[rank] > count))
            {
                return 1;
            }

            //�����ҵ��ƶ�û������,�����������˼�����
            return 0;
        }

        //���Ƿ��������
        internal static bool[] CanSetRank(MainForm mainForm, CurrentPoker currentPoker)
        {
            //���Ŀǰ��������
            int rank = mainForm.currentRank;
            bool[] suits = new bool[5] { false, false, false, false, false };

            //

            if (mainForm.showSuits == 0) //
            {
                if (rank != 53)
                {
                    if (currentPoker.Clubs[rank] > 0)
                    {
                        suits[3] = true;
                    }
                    else if (currentPoker.Diamonds[rank] > 0)
                    {
                        suits[2] = true;
                    }
                    else if (currentPoker.Peachs[rank] > 0)
                    {
                        suits[1] = true;
                    }
                    else if (currentPoker.Hearts[rank] > 0)
                    {
                        suits[0] = true;
                    }
                }
            }
            //���Է���
            if ((mainForm.showSuits == 1))
            {

                if (rank != 53)
                {
                    if (currentPoker.Clubs[rank] > 1)
                    {
                        //�Ƿ�ӹ̣��Ƿ������Է�
                        //
                        suits[3] = IsInvalidRank(mainForm,4);
                    }
                    else if (currentPoker.Diamonds[rank] > 1)
                    {
                        //�Ƿ�ӹ̣��Ƿ������Է�
                        suits[2] = IsInvalidRank(mainForm,3);
                    }
                    else if (currentPoker.Peachs[rank] > 1)
                    {
                        //�Ƿ�ӹ̣��Ƿ������Է�
                        suits[1] = IsInvalidRank(mainForm,2);
                    }
                    else if (currentPoker.Hearts[rank] > 1)
                    {
                        //�Ƿ�ӹ̣��Ƿ������Է�
                        suits[0] = IsInvalidRank(mainForm,1);
                    }
                }
            }

            if (mainForm.currentRank != 53)
            {
                if ((currentPoker.SmallJack == 2 || currentPoker.BigJack == 2) && (mainForm.showSuits < 3))
                {
                    //�Ƿ������Է�
                    //�Ƿ���������
                    suits[4] = IsInvalidRank(mainForm, 5);
                }
            }
            return suits;
        }

        private static bool IsInvalidRank2(MainForm form, int suit)
        {
            //�Ǽӹ�ʱ,�����Է�
            if ((suit != form.currentState.Suit) && (form.whoShowRank == 0) && (!form.gameConfig.CanMyRankAgain))  //����������Է�
            {
                return false;
            }

            if (!form.gameConfig.CanRankJack)  //�������������
            {
                return false;
            }

            return true;
        }

        private static bool IsInvalidRank(MainForm form,int suit)
        {
            //�Ƿ���Լӹ�
            if ((suit == form.currentState.Suit) && (form.whoShowRank == 0) && (!form.gameConfig.CanMyStrengthen))  //���������ӹ�
            {
                return false;
            }

            //�Ǽӹ�ʱ,�����Է�
            if ((suit != form.currentState.Suit) && (form.whoShowRank == 0) && (!form.gameConfig.CanMyRankAgain))  //����������Է�
            {
                return false;
            }

            return true;
        }

        #endregion // �����㷨


        #region �����㷨
        
        //�õ������Ӧ�ó�����
        internal static ArrayList ShouldSendedCards(MainForm mainForm, int whoseOrder, CurrentPoker[] currentPokers, ArrayList[] currentSendCard, int suit, int rank)
        {

            //�Ƿ���ò���㷨
            if (mainForm.UserAlgorithms[whoseOrder - 1] != null)
            {
                //��װ��
                string pokers = currentPokers[whoseOrder-1].getAllCards();
                string[] allSendCards = { "", "", "", "" };
                allSendCards[0] = mainForm.currentAllSendPokers[0].getAllCards();
                allSendCards[1] = mainForm.currentAllSendPokers[1].getAllCards();
                allSendCards[2] = mainForm.currentAllSendPokers[2].getAllCards();
                allSendCards[3] = mainForm.currentAllSendPokers[3].getAllCards();
                //
                IUserAlgorithm ua = (IUserAlgorithm)mainForm.UserAlgorithms[whoseOrder - 1];
                ArrayList result = ua.ShouldSendCards(whoseOrder, suit, rank, mainForm.currentState.Master, allSendCards, pokers);
                //�жϺϷ���
                bool b1 = TractorRules.IsInvalid(mainForm, currentSendCard, result, whoseOrder);
                bool b2 = TractorRules.CheckSendCards(mainForm, result,new ArrayList(), whoseOrder);

                //������
                if (b1 && b2)
                {
                   
                    int k = result.Count;

                    for (int i = 0; i < k; i++)
                    {
                        CommonMethods.SendCards(currentSendCard[whoseOrder - 1], currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (int)result[i]);
                    }
                }
                else
                {
                    mainForm.UserAlgorithms[whoseOrder - 1] = null;
                    ShouldSendedCardsAlgorithm.ShouldSendCards(mainForm, currentPokers, whoseOrder, currentSendCard[whoseOrder - 1]);
                }
            }
            else
            {
                ShouldSendedCardsAlgorithm.ShouldSendCards(mainForm, currentPokers, whoseOrder, currentSendCard[whoseOrder - 1]);
            }

            
            for (int i = 0; i < currentSendCard[whoseOrder - 1].Count; i++)
            {
                mainForm.currentAllSendPokers[whoseOrder - 1].AddCard((int)currentSendCard[whoseOrder - 1][i]);
            }
            return currentSendCard[whoseOrder - 1];
          

        }

        //�����ѳ����Լ�Ӧ�ó�����
        internal static ArrayList MustSendedCards(MainForm mainForm, int whoseOrder, CurrentPoker[] currentPokers, ArrayList[] currentSendCard, int suit, int rank, int count)
        {
            if (mainForm.UserAlgorithms[whoseOrder - 1] != null)
            {
                //��װ��
                string pokers = currentPokers[whoseOrder - 1].getAllCards();
                string[] allSendCards = { "", "", "", "" };
                allSendCards[0] = mainForm.currentAllSendPokers[0].getAllCards();
                allSendCards[1] = mainForm.currentAllSendPokers[1].getAllCards();
                allSendCards[2] = mainForm.currentAllSendPokers[2].getAllCards();
                allSendCards[3] = mainForm.currentAllSendPokers[3].getAllCards();
                //
                IUserAlgorithm ua = (IUserAlgorithm)mainForm.UserAlgorithms[whoseOrder - 1];
                ArrayList result = ua.MustSendCards(whoseOrder, suit, rank, mainForm.currentState.Master, mainForm.firstSend, allSendCards, currentSendCard, pokers);

                //�жϺϷ���
                bool b1 = TractorRules.IsInvalid(mainForm, currentSendCard, result, whoseOrder);
                bool b2 = TractorRules.CheckSendCards(mainForm, result, new ArrayList(), whoseOrder);

                if (b1 && b2)
                {
                    
                    int k = result.Count;
                    for (int i = 0; i < k; i++)
                    {
                        CommonMethods.SendCards(currentSendCard[whoseOrder - 1], currentPokers[whoseOrder - 1], mainForm.pokerList[whoseOrder - 1], (int)result[i]);
                    }
                }
                else
                {
                    mainForm.UserAlgorithms[whoseOrder - 1] = null;
                    MustSendCardsAlgorithm.MustSendCards(mainForm, currentPokers, whoseOrder, currentSendCard[whoseOrder - 1], count);
                }
            }
            else
            {
                MustSendCardsAlgorithm.MustSendCards(mainForm, currentPokers, whoseOrder, currentSendCard[whoseOrder - 1], count);
            }



            for (int i = 0; i < currentSendCard[whoseOrder - 1].Count; i++)
            {
                mainForm.currentAllSendPokers[whoseOrder - 1].AddCard((int)currentSendCard[whoseOrder - 1][i]);
            }

            return currentSendCard[whoseOrder - 1];
            
        }
        #endregion // �����㷨


        #region �����㷨
        internal static void Send8Cards(MainForm form, int user)
        {
           

            int suit = form.currentState.Suit;
            int rank = form.currentRank;

            int[] cardsTotal = new int[4];

            CurrentPoker cp = new CurrentPoker();
            cp.Suit = suit;
            cp.Rank = rank;
            cp = CommonMethods.parse(form.pokerList[user - 1],suit,rank);

            for (int i = 0; i < 13;i++)
            {
                
                    cardsTotal[0] += cp.HeartsNoRank[i];
               
                    cardsTotal[1] += cp.PeachsNoRank[i];
                
                    cardsTotal[2] += cp.DiamondsNoRank[i];
                
                    cardsTotal[3] += cp.ClubsNoRank[i];
                
            }
            if (rank == 12) //��AʱK���
            {
                
                    cardsTotal[0] -= cp.HeartsNoRank[11];
               
                    cardsTotal[1] -= cp.PeachsNoRank[11];
               
                    cardsTotal[2] -= cp.DiamondsNoRank[11];
                
                    cardsTotal[3] -= cp.ClubsNoRank[11];
                
            }
            else if (rank < 12)
            {
                
                    cardsTotal[0] -= cp.HeartsNoRank[12];
                
                    cardsTotal[1] -= cp.PeachsNoRank[12];
                
                    cardsTotal[2] -= cp.DiamondsNoRank[12];
               
                    cardsTotal[3] -= cp.ClubsNoRank[12];
               
            }


            if (form.gameConfig.BottomAlgorithm == 1)
            {
                //�㷨1:�۾�һ��
                Send8Cards1(form,form.pokerList[user - 1], form.send8Cards, cp, GetOrder(cardsTotal,suit),rank,suit);
            }
            else if (form.gameConfig.BottomAlgorithm == 2) //��ӹ
            {
                //�㷨2:�۾�������С��
                Send8Cards2(form, form.pokerList[user - 1], form.send8Cards, cp, GetOrder(cardsTotal, suit), rank, suit);
            }
            else if (form.gameConfig.BottomAlgorithm == 3) //����
            {
                //�㷨2:�۾�������С��
                Send8Cards3(form, form.pokerList[user - 1], form.send8Cards, cp, GetOrder(cardsTotal, suit), rank, suit);
            }
            else
            {
                Send8Cards1(form, form.pokerList[user - 1], form.send8Cards, cp, GetOrder(cardsTotal, suit), rank, suit);
            }

            form.initSendedCards();
            form.currentState.CurrentCardCommands = CardCommands.DrawMySortedCards;
        }

        /// <summary>
        /// Send8s the cards1.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="sendCards">The send cards.</param>
        /// <param name="cp">The cp.</param>
        /// <param name="suits">The suits.</param>
        /// <param name="rank">The rank.</param>
        /// <param name="suit">The suit.</param>
        internal static void Send8Cards1(MainForm form, ArrayList list,ArrayList sendCards,CurrentPoker cp,int[] suits,int rank,int suit)
        {
            int previous = 0, next = 0;

            int[] suitCards = cp.GetSuitAllCards(suits[0]);

            GetShouldSend8Cards(list, sendCards, suits[0], rank, suit, previous,next, suitCards);

            if (sendCards.Count < 8)
            {
                suitCards = cp.GetSuitAllCards(suits[1]);
                GetShouldSend8Cards(list, sendCards, suits[1], rank, suit, previous, next, suitCards);
            }
            if (sendCards.Count < 8)
            {
                suitCards = cp.GetSuitAllCards(suits[2]);
                GetShouldSend8Cards(list, sendCards, suits[2], rank, suit, previous, next, suitCards);
            }
            if (sendCards.Count < 8)
            {
                suitCards = cp.GetSuitAllCards(suits[3]);
                GetShouldSend8Cards(list, sendCards, suits[3], rank, suit, previous, next, suitCards);
            }
        }

        internal static void Send8Cards3(MainForm form, ArrayList list, ArrayList sendCards, CurrentPoker cp, int[] suits, int rank, int suit)
        {
            int previous = 0, next = 0;

            int[][] suitCards = new int[4][];

            suitCards[0] = cp.GetSuitAllCards(suits[0]);
            suitCards[1] = cp.GetSuitAllCards(suits[1]);
            suitCards[2] = cp.GetSuitAllCards(suits[2]);
            suitCards[3] = cp.GetSuitAllCards(suits[3]);

            for (int i = 0; i < 13; i++)
            {
                //
                if (i == 0)
                {
                    previous = 0;
                }
                else if (rank == (i - 1))
                {
                    if (i == 1)
                    {
                        previous = 0;
                    }
                    else
                    {
                        previous = suitCards[0][i - 2];
                    }
                }
                else
                {
                    previous = suitCards[0][i - 1];
                }
                //
                if (i == 12)
                {
                    next = 0;
                }
                else if (rank == (i + 1))
                {
                    if (i == 11)
                    {
                        next = 0;
                    }
                    else
                    {
                        next = suitCards[0][i + 2];
                    }
                }
                else
                {
                    next = suitCards[0][i + 1];

                }


                if (IsCanSend(i, suitCards[0][i], previous, next, suit))
                {
                   
                    
                    list.Remove((suits[0] - 1) * 13 + i);
                    sendCards.Add((suits[0] - 1) * 13 + i);
                    cp.RemoveCard((suits[0] - 1) * 13 + i);
                }

                if (sendCards.Count >= 8)
                {
                    return;
                }
                //2
                if (i == 0)
                {
                    previous = 0;
                }
                else if (rank == (i - 1))
                {
                    if (i == 1)
                    {
                        previous = 0;
                    }
                    else
                    {
                        previous = suitCards[2][i - 2];
                    }
                }
                else
                {
                    previous = suitCards[2][i - 1];
                }
                //
                if (i == 12)
                {
                    next = 0;
                }
                else if (rank == (i + 1))
                {
                    if (i == 11)
                    {
                        next = 0;
                    }
                    else
                    {
                        next = suitCards[2][i + 2];
                    }
                }
                else
                {
                    next = suitCards[2][i + 1];

                }

                if (IsCanSend(i, suitCards[1][i], previous, next, suit))
                {
                    
                    list.Remove((suits[1] - 1) * 13 + i);
                    sendCards.Add((suits[1] - 1) * 13 + i);
                    cp.RemoveCard((suits[1] - 1) * 13 + i);
                }

                if (sendCards.Count >= 8)
                {
                    return;
                }
                //3.
                if (i == 0)
                {
                    previous = 0;
                }
                else if (rank == (i - 1))
                {
                    if (i == 1)
                    {
                        previous = 0;
                    }
                    else
                    {
                        previous = suitCards[2][i - 2];
                    }
                }
                else
                {
                    previous = suitCards[2][i - 1];
                }
                //
                if (i == 12)
                {
                    next = 0;
                }
                else if (rank == (i + 1))
                {
                    if (i == 11)
                    {
                        next = 0;
                    }
                    else
                    {
                        next = suitCards[2][i + 2];
                    }
                }
                else
                {
                    next = suitCards[2][i + 1];

                }

                if (IsCanSend(i, suitCards[2][i], previous, next, suit))
                {
                   
                    list.Remove((suits[2] - 1) * 13 + i);
                    sendCards.Add((suits[2] - 1) * 13 + i);
                    cp.RemoveCard((suits[2] - 1) * 13 + i);
                }

                if (sendCards.Count >= 8)
                {
                    return;
                }

                //4.
                if (sendCards.Count >= 8)
                {
                    return;
                }
                if (suit == 5)
                {
                    if (i == 0)
                    {
                        previous = 0;
                    }
                    else if (rank == (i - 1))
                    {
                        if (i == 1)
                        {
                            previous = 0;
                        }
                        else
                        {
                            previous = suitCards[3][i - 2];
                        }
                    }
                    else
                    {
                        previous = suitCards[3][i - 1];
                    }
                    //
                    if (i == 12)
                    {
                        next = 0;
                    }
                    else if (rank == (i + 1))
                    {
                        if (i == 11)
                        {
                            next = 0;
                        }
                        else
                        {
                            next = suitCards[3][i + 2];
                        }
                    }
                    else
                    {
                        next = suitCards[3][i + 1];

                    }

                    if (IsCanSend(i, suitCards[3][i], previous, next, suit))
                    {
                        list.Remove((suits[3] - 1) * 13 + i);
                        sendCards.Add((suits[3] - 1) * 13 + i);
                        cp.RemoveCard((suits[3] - 1) * 13 + i);
                    }

                    if (sendCards.Count >= 8)
                    {
                        return;
                    }
                }
                
            }
           
            //last
            if (suit < 5)
            {
                for (int i = 0; i < 13; i++)
                {
                    if (sendCards.Count >= 8)
                    {
                        return;
                    }

                    //
                    if (i == 0)
                    {
                        previous = 0;
                    }
                    else if (rank == (i - 1))
                    {
                        if (i == 1)
                        {
                            previous = 0;
                        }
                        else
                        {
                            previous = suitCards[3][i - 2];
                        }
                    }
                    else
                    {
                        previous = suitCards[3][i - 1];
                    }
                    //
                    if (i == 12)
                    {
                        next = 0;
                    }
                    else if (rank == (i + 1))
                    {
                        if (i == 11)
                        {
                            next = 0;
                        }
                        else
                        {
                            next = suitCards[3][i + 2];
                        }
                    }
                    else
                    {
                        next = suitCards[3][i + 1];

                    }


                    if (IsCanSend(i, suitCards[3][i], previous, next, suit))
                    {
                       
                        list.Remove((suits[3] - 1) * 13 + i);
                        sendCards.Add((suits[3] - 1) * 13 + i);
                        cp.RemoveCard((suits[3] - 1) * 13 + i);
                    }

                    if (sendCards.Count >= 8)
                    {

                        return;
                    }
                }
            }

            if (sendCards.Count >= 8)
            {
                return;
            }
            else
            {
                Send8Cards3(form, list, sendCards,cp, suits, rank, suit);
            }
        }

        internal static void Send8Cards2(MainForm form, ArrayList list, ArrayList sendCards, CurrentPoker cp, int[] suits, int rank, int suit)
        {
            int previous = 0, next = 0;

            int[] suitCards = cp.GetSuitAllCards(suits[0]);

            GetShouldSend8CardsNoScores(list, sendCards, suits[0], rank, suit, previous, next, suitCards);

            if (sendCards.Count < 8)
            {
                suitCards = cp.GetSuitAllCards(suits[1]);
                GetShouldSend8CardsNoScores(list, sendCards, suits[1], rank, suit, previous, next, suitCards);
            }
            if (sendCards.Count < 8)
            {
                suitCards = cp.GetSuitAllCards(suits[2]);
                GetShouldSend8CardsNoScores(list, sendCards, suits[2], rank, suit, previous, next, suitCards);
            }
            if (sendCards.Count < 8)
            {
                suitCards = cp.GetSuitAllCards(suits[3]);
                GetShouldSend8CardsNoScores(list, sendCards, suits[3], rank, suit, previous, next, suitCards);
            }

            if (sendCards.Count < 8)
            {
                suitCards = cp.GetSuitAllCards(suits[0]);
                GetShouldSend8Cards(list, sendCards, suits[0], rank, suit, previous, next, suitCards);
            }
            if (sendCards.Count < 8)
            {
                suitCards = cp.GetSuitAllCards(suits[1]);
                GetShouldSend8Cards(list, sendCards, suits[1], rank, suit, previous, next, suitCards);
            }
            if (sendCards.Count < 8)
            {
                suitCards = cp.GetSuitAllCards(suits[2]);
                GetShouldSend8Cards(list, sendCards, suits[2], rank, suit, previous, next, suitCards);
            }
            if (sendCards.Count < 8)
            {
                suitCards = cp.GetSuitAllCards(suits[3]);
                GetShouldSend8Cards(list, sendCards, suits[3], rank, suit, previous, next, suitCards);
            }

            //�ƻ�����
            if (sendCards.Count < 8)
            {
               Send8Cards3(form, list, sendCards, cp, suits, rank, suit);
            }
        }

        private static void GetShouldSend8Cards(ArrayList list, ArrayList sendCards, int thisSuit, int rank, int suit, int previous, int next, int[] suitCards)
        {
            for (int i = 0; i < 13; i++)
            {
               

                if (sendCards.Count >= 8)
                {
                    break;
                }

                if (i == 0)
                {
                    previous = 0;
                }
                else if (rank == (i - 1))
                {
                    if (i == 1)
                    {
                        previous = 0;
                    }
                    else
                    {
                        previous = suitCards[i - 2];
                    }
                }
                else
                {
                    previous = suitCards[i - 1];
                }
                //
                if (i == 12)
                {
                    next = 0;
                }
                else if (rank == (i + 1))
                {
                    if (i == 11)
                    {
                        next = 0;
                    }
                    else
                    {
                        next = suitCards[i + 2];
                    }
                }
                else
                {
                    next = suitCards[i + 1];

                }


                if (IsCanSend(i, suitCards[i], previous, next, suit))
                {
                    list.Remove((thisSuit - 1) * 13 + i);
                    sendCards.Add((thisSuit - 1) * 13 + i);
                }

            }

        }



        private static void GetShouldSend8CardsNoScores(ArrayList list, ArrayList sendCards, int thisSuit, int rank, int suit, int previous,int next, int[] suitCards)
        {
            for (int i = 0; i < 13; i++)
            {
                if (i == 8 || i == 11)
                {
                    continue;
                }

                if (sendCards.Count >= 8)
                {
                    break;
                }

                if (i == 0)
                {
                    previous = 0;
                }
                else if (rank == (i - 1))
                {
                    if (i == 1)
                    {
                        previous = 0;
                    }
                    else
                    {
                        previous = suitCards[i - 2];
                    }
                }
                else
                {
                    previous = suitCards[i - 1];
                }
                //
                if (i == 12)
                {
                    next = 0;
                }
                else if (rank == (i + 1))
                {
                    if (i == 11)
                    {
                        next = 0;
                    }
                    else
                    {
                        next = suitCards[i + 2];
                    }
                }
                else
                {
                    next = suitCards[i + 1];

                }


                if (IsCanSend(i, suitCards[i], previous, next, suit))
                {
                    list.Remove((thisSuit - 1) * 13 + i);
                    sendCards.Add((thisSuit - 1) * 13 + i);
                }
                
            }
            
        }

        private static bool IsCanSend(int number, int thisTotal,int previous,int next,int suit)
        {
            if (thisTotal == 0)
            {
                return false;
            }
            if (suit == 12)
            {
                if (number == 11)
                {
                    return false;
                }
            }
            else if (suit < 12)
            {
                if (number == 12)
                {
                    return false;
                }
            }

            //

            if (thisTotal == 2) //����������
            {
                if (number >= 8)
                {
                    return false;
                }
                if (previous == 2 || next == 2) //������
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return true;
        }


        private static int[] GetOrder(int[] cardsTotals, int suit)
        {
            int[] result = {0,1,2,3};
            if (suit ==1)
            {
                result[3] = 0;
                result[0] = 3;
            }
            else if (suit == 2)
            {
                result[3] = 1;
                result[1] = 3;
            }
            else if (suit == 3)
            {
                result[3] = 2;
                result[2] = 3;
            }

            if (suit < 5)
            {
                if (cardsTotals[0] < cardsTotals[1])
                {
                    int tmp = result[0];
                    result[0] = result[1];
                    result[1] = tmp;
                }
                if (cardsTotals[1] < cardsTotals[2])
                {
                    int tmp = result[1];
                    result[1] = result[2];
                    result[2] = tmp;
                }
                if (cardsTotals[0] < cardsTotals[1])
                {
                    int tmp = result[0];
                    result[0] = result[1];
                    result[1] = tmp;
                }
            }
            else
            {
                if (cardsTotals[0] < cardsTotals[1])
                {
                    int tmp = result[0];
                    result[0] = result[1];
                    result[1] = tmp;
                }
                if (cardsTotals[1] < cardsTotals[2])
                {
                    int tmp = result[1];
                    result[1] = result[2];
                    result[2] = tmp;
                }
                if (cardsTotals[2] < cardsTotals[3])
                {
                    int tmp = result[2];
                    result[2] = result[3];
                    result[3] = tmp;
                }
                if (cardsTotals[1] < cardsTotals[2])
                {
                    int tmp = result[1];
                    result[1] = result[2];
                    result[2] = tmp;
                }
                if (cardsTotals[0] < cardsTotals[1])
                {
                    int tmp = result[0];
                    result[0] = result[1];
                    result[1] = tmp;
                }
            }

            //
            for (int i = 0; i < 4; i++)
            {
                result[i]++;
            }

            return result;
        }

        #endregion // �����㷨


    }
}
