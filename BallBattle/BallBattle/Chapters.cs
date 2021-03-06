﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BallBattle.Model.Interface;
using BallBattle.Factory;

namespace BallBattle
{

    /*
     *关卡类,用于存储和控制所有关卡相关内容,
     * 包括
     * 1.关卡要求分数
     * 2,关卡的颜色
     * 3.关卡中的球的基础速度
     * 4.关卡中球生成的频率
     * 5.关卡中球的种类
     * 6.对应球的种类的生成比例
     * 
     */
    class Chapters
    {
        private class Chapter
        {
            public int point;//关卡要求分数
            public Color color;//关卡初始颜色
            public int speed;//关卡的球的基准速度
            public int genRate;//生成频率
            public List<BallFactory> ballsf;//工厂对象list
            public List<int> rates;
            public int topLimit;
            public int lowLimit;



            public Chapter(int p, Color c, int speed, int genRate, int lLimit,int tLimit, List<BallFactory> ballsf, List<int> rates)
            {
                point = p;
                color = c;
                this.speed = speed;
                this.genRate = genRate;
                this.ballsf = ballsf;
                this.rates = rates;
                this.topLimit = tLimit;
                this.lowLimit = lLimit;

            }

        }

        private int level = 1;//当前关卡数

        private static List<Chapter> chapters;//关卡信息

        private static Chapters instance = new Chapters();

        private int times = 0;


        BallFactory f1;//工厂类
        BallFactory f2;
        BallFactory f3;
        BallFactory f4;
        BallFactory f5;
        BallFactory f6;


        public static Chapters getInstance()
        {
            return instance;
        }

        private Chapters()
        {
            //初始化关卡信息


            initFactory();
            chapters = new List<Chapter>{
                                          new Chapter(0,Color.White,2,50,-1,2,new List<BallFactory>{f1},new List<int>{1}),                                        
                                          new Chapter(50,Color.Blue,3,20,-10,30,new List<BallFactory>{f1,f4},new List<int>{8,3}),
                                          new Chapter(100,Color.Red,4,50,-1,1,new List<BallFactory>{f2,f3},new List<int>{1,1}), 
                                          new Chapter(150,Color.Red,5,50,-1,2,new List<BallFactory>{f1,f3},new List<int>{1,1}), 
                                          new Chapter(200,Color.Red,6,50,-1,2,new List<BallFactory>{f1,f5},new List<int>{1,20}), 
                                          new Chapter(250,Color.Red,8,50,-1,2,new List<BallFactory>{f1,f2,f2,f3,f4,f5,f6},new List<int>{1,1,1,1,1,1}), 
                                        };
            init();
        }


        public void initFactory()
        {
            //初始化工厂

            //f1 生产随机位置和随机方向的鱼,吃掉后加分
            f1 = new BallFactory(Resourse.getInstance().baseBallTexture, Color.White, "", "RandomRoad");

            //f2 生产幕布式的鱼,吃掉后加分
            f2 = new BallFactory(Resourse.getInstance().baseBallTexture, Color.White, "", "LinerRoad");

            //f3 生产冲锋式的鱼,会向玩家冲去,吃掉后加分
            f3 = new BallFactory(Resourse.getInstance().baseBallTexture, Color.White, "", "Rush");

            //f4 生产随机位置和随机方向的鱼,吃掉后减分
            f4 = new BallFactory(Resourse.getInstance().baseBallTexture, Color.White, "SubImpact", "RandomRoad");

            //f5 生产幕布式的鱼,吃掉后减分
            f5 = new BallFactory(Resourse.getInstance().baseBallTexture, Color.White, "SubImpact", "LinerRoad");

            //f6  生产冲锋式的鱼,会向玩家冲去,吃掉后减分
            f6 = new BallFactory(Resourse.getInstance().baseBallTexture, Color.White, "SubImpact", "Rush");
        }

        public void init()
        {
            level = 0;
            times = 0;

        }
        private Chapter getChapter(int num)
        {
            if (num >= chapters.Count)
            {
                Game1.gameState = 3;
                return chapters[0];
            }
            return chapters[num];
        }

        private Chapter getCurrentChapter()
        {
            return getChapter(level);
        }


        public int getLevel()
        {
            return level;
        }

        public int getCurrentChapterPoint()
        {
            return getCurrentChapter().point;
        }

        public int getCurrentChapterSpeed()
        {
            return getCurrentChapter().speed;
        }

        public int getCurrentChapterGenRate()
        {
            return getCurrentChapter().genRate;
        }

        public int getCurrentChapterTopLimit()
        {
            return getCurrentChapter().topLimit;
        }

        public int getCurrentChapterLowLimit()
        {
            return getCurrentChapter().lowLimit;
        }

        public Color getCurrentChapterColor()
        {
            return getCurrentChapter().color;
        }

        public BaseBall getBaseBall()
        {
            times++;
            if (times < getCurrentChapterGenRate())
            {
                return null;
            }
            times = 0;

            BaseBall gBall = null;


            int sum = 0;
            Random r = new Random();//随机数,用于随机取一个球
            List<int> rates = getCurrentChapter().rates;

            foreach (int each in rates)
            {
                sum += each;
            }

            int ran = r.Next(0, sum);

            int datVal = r.Next(getCurrentChapterLowLimit(),getCurrentChapterTopLimit())*2;

            int a = 0;
            for (int i = 0; i < rates.Count; i++)
            {

                if (a <= ran && (a + rates[i]) >= ran)
                {
                    gBall = getCurrentChapter().ballsf[i].built(PlayerBall.getInstance().getVal() + datVal);
                    break;
                }
                a += rates[i];
            }



            gBall.setSpeed(getCurrentChapterSpeed());


            return gBall;
        }

        public void check()
        {
            PlayerBall player = PlayerBall.getInstance();

            if (ScoreBoard.getInstance().addScore(0) >= getCurrentChapterPoint())
            {
                level++;
                Resourse.getInstance().levelUp.Play();
                if(level>=2){
                    PlayerBall.getInstance().addVal(-50,false);
                }
            }

            if (level == chapters.Count)
            {
                Game1.gameState = 3;
            }


        }



    }
}
