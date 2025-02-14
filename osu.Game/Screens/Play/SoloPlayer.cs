// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Diagnostics;
using osu.Game.Online.API;
using osu.Game.Online.Rooms;
using osu.Game.Online.Solo;
using osu.Game.Rulesets;
using osu.Game.Scoring;

namespace osu.Game.Screens.Play
{
    public class SoloPlayer : SubmittingPlayer
    {
        public SoloPlayer()
            : this(null)
        {
        }

        protected SoloPlayer(PlayerConfiguration configuration = null)
            : base(configuration)
        {
        }

        protected override APIRequest<APIScoreToken> CreateTokenRequest()
        {
            if (!(Beatmap.Value.BeatmapInfo.OnlineID is int beatmapId))
                return null;

            if (!(Ruleset.Value.ID is int rulesetId) || Ruleset.Value.ID > ILegacyRuleset.MAX_LEGACY_RULESET_ID)
                return null;

            return new CreateSoloScoreRequest(beatmapId, rulesetId, Game.VersionHash);
        }

        protected override bool HandleTokenRetrievalFailure(Exception exception) => false;

        protected override APIRequest<MultiplayerScore> CreateSubmissionRequest(Score score, long token)
        {
            var beatmap = score.ScoreInfo.BeatmapInfo;

            Debug.Assert(beatmap.OnlineID != null);

            int beatmapId = beatmap.OnlineID.Value;

            return new SubmitSoloScoreRequest(beatmapId, token, score.ScoreInfo);
        }
    }
}
