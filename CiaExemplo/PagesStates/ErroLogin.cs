﻿using JsonDocumentsManager;
using StatesAndEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheRobot;

namespace CiaExemplo.PagesStates;

public class ErroLogin : BaseState
{
    public ErroLogin(Robot robot, BaseOrcamento baseOrcamento, ResultJsonDocument resultJson) : base("ErroLogin", robot, baseOrcamento, resultJson)
    {
    }

    public override void Execute()
    {
        Console.WriteLine("Você digitou a senha errada");
    }
}