namespace Carto.Systems
{
    using Colossal.Logging;
    using Game;
    using Game.Audio;
    using Game.Prefabs;
    using Unity.Entities;
    using Unity.Mathematics;

    /// <summary>
    /// The system instance that manages the sound effects, inheriting from GameSystemBase.
    /// （管理音效的系統實例，其特性繼承自 GameSystemBase 。）
    /// </summary>
    public partial class AudioSystem : GameSystemBase
    {
        private static readonly ILog m_Log = Instance.Log;
        private AudioManager _audio = Instance.AudioManager;

        // The entity query instance that searches for several instances, using Unity.Entities.EntityQuery.
        // （用於搜尋數種實例的Unity實體查詢實例。）
        private static EntityQuery _soundQuery;

        /// <summary>
        /// This event triggers when the system is created.
        /// （這是當系統實例被創造時，會被觸發的事件。）
        /// </summary>
        protected override void OnCreate()
        {
            _soundQuery = GetEntityQuery(new EntityQueryDesc
            {
                All = new ComponentType[]
                {
                    ComponentType.ReadOnly<ToolUXSoundSettingsData>()
                }
            });
            
            base.OnCreate();
            m_Log.Debug("AudioSystem instance created. 音效系統實例創造完成。");
        }

        /// <summary>
        /// The enum storing tool sound effects.
        /// （儲存工具音效的枚舉。）
        /// </summary>
        public enum Sound
        {
            Completion
        }

        /// <summary>
        /// This event triggers when the system is updated.
        /// （這是當系統實例被更新時，會被觸發的事件。）
        /// </summary>
        protected override void OnUpdate() { }

        /// <summary>
        /// This event triggers when the system is destroyed.
        /// （這是當系統實例被銷毀時，會被觸發的事件。）
        /// </summary>
        protected override void OnDestroy() { base.OnDestroy(); }

        /// <summary>
        /// Play the sound at the given volume.
        /// （以指定的音量撥放音效。）
        /// </summary>
        /// <param name="sound">The sound.（音效。）</param>
        /// <param name="volume">The volume of the sound.（音效的音量。）</param>
        public void PlaySound(Sound sound, float volume = 1f)
        {
            Entity soundEntity = Entity.Null;

            switch (sound)
            {
                case Sound.Completion:
                    soundEntity = _soundQuery.GetSingleton<ToolUXSoundSettingsData>().m_TutorialCompletedSound; // ♩♫♫
                    break;
            }
            
            _audio.PlayUISound(soundEntity, volume);
        }
    }
}
