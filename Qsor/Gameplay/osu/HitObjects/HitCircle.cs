﻿using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Logging;
using osuTK;

namespace Qsor.Gameplay.osu.HitObjects
{
    public class HitCircle : HitObject
    {
        private Sprite _hitCircle;
        private Sprite _approachCircle;
        private Sprite _hitCircleOverlay;

        public override HitObjectType Type => HitObjectType.Circle;

        public bool AutoHide = true;
        

        [BackgroundDependencyLoader]
        private void Load(TextureStore store)
        {
            _hitCircle = new Sprite
            {
                Texture = store.Get("hitcircle.png"),
                
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                Colour = HitObjectColour,
                
                Size = new Vector2(64, 64),
            };

            _approachCircle = new Sprite
            {
                Texture = store.Get("approachcircle.png"),
                
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                
                Scale = new Vector2(2),
                
                Size = new Vector2(126, 126),
            };

            _hitCircleOverlay = new Sprite
            {
                Texture = store.Get("hitcircleoverlay.png"),
                
                Anchor = Anchor.Centre,
                Origin = Anchor.Centre,
                
                Size = new Vector2(64, 64)
            };

            Add(_hitCircle);
            Add(_approachCircle);
            Add(_hitCircleOverlay);

            Alpha = 0;
            Size = new Vector2(128, 128);
            BindableScale.ValueChanged += val =>
                Scale = new Vector2((1.0f - 0.7f * ((float) val.NewValue - 5) / 5) / 2);

            BindableProgress.ValueChanged += prog =>
            {
                if (AutoHide)
                    Hide();
            };
        }

        private bool _turnedHidden;
        
        public override void Hide()
        {
            if (_turnedHidden)
                return;
            
            _turnedHidden = true;
            
            //base.Hide();
            _approachCircle.FadeTo(0, 100);
            _hitCircleOverlay.ScaleTo(2f, 100);
            _hitCircleOverlay.FadeOutFromOne(200);
            _hitCircle.FadeOutFromOne(200);
            
            this.FadeOutFromOne(200)
                .Finally(o =>
            {
                (Parent as Container)?.Remove(this); // TODO: Fix
            });
        }

        public override void Show()
        {
            //base.Show();
            _turnedHidden = false;

            this.FadeInFromZero(200);
            _approachCircle.ScaleTo((float) BindableScale.Value, 600);
        }

        protected override void Update()
        {
            base.Update();
        }

        public HitCircle(Beatmap beatmap, Vector2 position) : base(beatmap, position)
        {
        }
    }
}