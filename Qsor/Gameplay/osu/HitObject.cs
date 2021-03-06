﻿using System;
using System.Diagnostics.CodeAnalysis;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Logging;
using osuTK;

namespace Qsor.Gameplay.osu
{
    [Flags]
    [SuppressMessage("ReSharper", "ShiftExpressionRealShiftCountIsZero")]
    public enum HitObjectType
    {
        Circle   = 1 << 0,
        Slider   = 1 << 1,
        NewCombo = 1 << 2,
        Spinner  = 1 << 3
    }

    public abstract class HitObject : Container, IHasEndTime
    {
        public double BeginTime;
        public virtual double EndTime => BeginTime + 600;

        public virtual double Duration => EndTime - BeginTime;
        public float HitObjectSize { get; }

        public ColourInfo
            HitObjectColour; // we do not use Colour, we use HitObjectColour instead, as Colour would Colour the whole HitCircle. (in theory, not tested)

        public abstract HitObjectType Type { get; }

        public TimingPoint TimingPoint { get; set; }

        //[Resolved]
        //private BeatmapManager BeatmapManager { get; set; }

        public Beatmap Beatmap { get; }

        public BindableDouble BindableScale = new BindableDouble();
        public BindableDouble BindableProgress = new BindableDouble();
        
        public HitObject(Beatmap beatmap, Vector2 position)
        {
            Position = position;
            
            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;

            Beatmap = beatmap;
            
            BindableProgress.Default = 0;
            BindableScale.Default = (1.0f - 0.7f * ((float) Beatmap.Difficulty.CircleSize - 5) / 5) / 2;
            
            BindableProgress.SetDefault();
            BindableScale.SetDefault();
        }
        
        protected override void Update()
        {
            Console.WriteLine(Clock.CurrentTime);
            BindableProgress.Value = Math.Clamp((Clock.CurrentTime - BeginTime) / Duration, 0, 1);
        }
    }
}