﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RexReader {
    public class Tile {
        /// <summary>
        /// ASCII-like code of the character. ASCII-like because REXPaint uses the default libtcod-style
        /// fonts, where the normally-invisible ASCII control codes are mapped to useful things (like smile guy).
        /// Ultimately it is up to you to map this into something usable for your game. Normally printable
        /// ASCII codes should be what you expect.
        /// </summary>
        public byte CharacterCode { get; set; }
        public byte BackgroundRed { get; set; }
        public byte BackgroundGreen { get; set; }
        public byte BackgroundBlue { get; set; }
        public byte ForegroundRed { get; set; }
        public byte ForegroundGreen { get; set; }
        public byte ForegroundBlue { get; set; }
    }
}
