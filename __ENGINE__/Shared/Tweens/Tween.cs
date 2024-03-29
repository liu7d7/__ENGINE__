﻿using OpenTK.Mathematics;

namespace __ENGINE__.Shared.Tweens
{
    public class Tween : BaseTween
    {
        private readonly Animations.Animation _animation;
        public override float output() => MathHelper.Clamp(infinite ? _animation(duration, (Environment.TickCount - lastActivation) % duration) : _animation(duration, Environment.TickCount - lastActivation), 0, 1);
        public override float output_at(float time)
        {
            if (time < lastActivation)
            {
                return 0;
            }

            if (time > lastActivation + duration && !infinite)
            {
                return 1;
            }

            return MathHelper.Clamp(infinite ? _animation(duration, (time - lastActivation) % duration) : _animation(duration, time - lastActivation), 0, 1);
        }

        public Tween(Animations.Animation animation, float duration, bool repeating)
        {
            lastActivation = Environment.TickCount;
            _animation = animation;
            infinite = repeating;
            base.duration = duration;
        }

        public override bool done() => Environment.TickCount - lastActivation > duration;
        public bool past_half => Environment.TickCount - lastActivation > duration / 2;
    }
}