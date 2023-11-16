﻿using System;
using CrashKonijn.Goap.Classes.Validators;
using CrashKonijn.Goap.Core.Interfaces;
using CrashKonijn.Goap.Editor.Elements;
using UnityEngine.UIElements;

namespace CrashKonijn.Goap.Editor.NodeViewer.Drawers
{
    public class WorldDataDrawer : VisualElement
    {
        public WorldDataDrawer(IWorldData worldData)
        {
            this.name = "world-data";
            
            var card = new Card((card) =>
            {
                card.Add(new Header("Conditions"));
                
                var root = new VisualElement();
                
                card.schedule.Execute(() =>
                {
                    root.Clear();
                    
                    foreach (var (key, state) in worldData.States)
                    {
                        root.Add(new Label(this.GetText(key, state)));
                    }
                }).Every(500);
                
                card.Add(root);
            });
            
            this.Add(card);
        }
        
        private string GetText(Type worldKey, int value)
        {
            return  $"{worldKey.GetGenericTypeName()} ({value})";
        }
    }
}