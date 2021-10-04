// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Linq;
using osu.Framework.Localisation;
using osu.Game.Database;
using osu.Game.Rulesets;

#nullable enable

namespace osu.Game.Beatmaps
{
    /// <summary>
    /// A single beatmap difficulty.
    /// </summary>
    public interface IBeatmapInfo : IHasOnlineID
    {
        /// <summary>
        /// The user-specified name given to this beatmap.
        /// </summary>
        string DifficultyName { get; }

        /// <summary>
        /// The metadata representing this beatmap. May be shared between multiple beatmaps.
        /// </summary>
        IBeatmapMetadataInfo? Metadata { get; }

        /// <summary>
        /// The difficulty settings for this beatmap.
        /// </summary>
        IBeatmapDifficultyInfo Difficulty { get; }

        /// <summary>
        /// The beatmap set this beatmap is part of.
        /// </summary>
        IBeatmapSetInfo BeatmapSet { get; }

        /// <summary>
        /// The playable length in milliseconds of this beatmap.
        /// </summary>
        double Length { get; }

        /// <summary>
        /// The most common BPM of this beatmap.
        /// </summary>
        double BPM { get; }

        /// <summary>
        /// The SHA-256 hash representing this beatmap's contents.
        /// </summary>
        string Hash { get; }

        /// <summary>
        /// MD5 is kept for legacy support (matching against replays etc.).
        /// </summary>
        string MD5Hash { get; }

        /// <summary>
        /// The ruleset this beatmap was made for.
        /// </summary>
        IRulesetInfo Ruleset { get; }

        /// <summary>
        /// The basic star rating for this beatmap (with no mods applied).
        /// </summary>
        double StarRating { get; }

        /// <summary>
        /// A user-presentable display title representing this metadata.
        /// </summary>
        string DisplayTitle => $"{Metadata} {versionString}".Trim();

        /// <summary>
        /// A user-presentable display title representing this beatmap, with localisation handling for potentially romanisable fields.
        /// </summary>
        RomanisableString DisplayTitleRomanisable
        {
            get
            {
                var metadata = closestMetadata.DisplayTitleRomanisable;

                return new RomanisableString($"{metadata.GetPreferred(true)} {versionString}".Trim(), $"{metadata.GetPreferred(false)} {versionString}".Trim());
            }
        }

        string[] SearchableTerms => new[]
        {
            DifficultyName
        }.Concat(closestMetadata.SearchableTerms).Where(s => !string.IsNullOrEmpty(s)).ToArray();

        private string versionString => string.IsNullOrEmpty(DifficultyName) ? string.Empty : $"[{DifficultyName}]";

        // temporary helper methods until we figure which metadata should be where.
        private IBeatmapMetadataInfo closestMetadata => (Metadata ?? BeatmapSet.Metadata)!;
    }
}
