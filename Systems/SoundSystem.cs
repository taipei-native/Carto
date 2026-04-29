using Carto.Domain;
using Colossal.Logging;
using Game;
using Game.Audio;
using Game.Prefabs;
using Unity.Entities;

namespace Carto.Systems
{
    /// <summary>
    /// The system that manages sounds.
    /// （管理聲音的系統。）
    /// </summary>
    public partial class SoundSystem : GameSystemBase
    {
        /// <summary>
        /// The system managing sound effects.（管理音效的系統。）<br/>
        /// See <see cref="Instance.Audio"/> for more information.
        /// </summary>
        static readonly AudioManager _audio = Instance.Audio;

        /// <summary>
        /// Mod's logger.（模組的記錄器。）<br/>
        /// See <see cref="Instance.Log"/> for more information.
        /// </summary>
        static readonly ILog _log = Instance.Log;

        /// <summary>
        /// The query for sound effects.（音效的查詢。）
        /// </summary>
        static EntityQuery _soundQuery;

        /// <summary>
        /// The event triggered when the system instance is created.
        /// （當系統實例被創造時觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _soundQuery = GetEntityQuery(new EntityQueryDesc()
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<ToolUXSoundSettingsData>()
                }
            });
            
            base.OnCreate();
            _log.Debug("SoundSystem instance created. 音效系統實例創造完成。");
        }

        /// <summary>
        /// The event triggered when the system instance is destroyed.
        /// （當系統實例被銷毀時所觸發的事件。）
        /// </summary>
        protected override void OnDestroy() { base.OnDestroy(); }

        /// <summary>
        /// The event triggered when the system instance is updated.
        /// （當系統實例被更新時觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// Play a sound.
        /// （播放音效。）
        /// </summary>
        /// <param name="sound">The sound type.（音效的種類。）</param>
        /// <param name="volume">The sound volume.（音量。）</param>
        public void Play(Sound sound, float volume = 1f)
        {
            Entity soundEntity = Entity.Null;
            ToolUXSoundSettingsData sounds = _soundQuery.GetSingleton<ToolUXSoundSettingsData>();

            switch (sound)
            {
                case Sound.Completion:
                    soundEntity = sounds.m_TutorialCompletedSound;
                    break;
                
                default:
                    break;
            }

            if (soundEntity != Entity.Null) _audio.PlayUISound(soundEntity, volume);
        }
    }
}