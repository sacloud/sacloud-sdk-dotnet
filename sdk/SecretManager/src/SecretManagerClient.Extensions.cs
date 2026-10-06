using Microsoft.Extensions.Logging;
using Microsoft.Kiota.Abstractions;
using System;
using System.Collections.Generic;

namespace Sacloud.Sdk.SecretManager
{
    public partial class SecretManagerClient
    {
        /// <summary>
        /// サクラクラウドのゾーンを表します。
        /// </summary>
        public enum SakuraCloudZone
        {
            /// <summary>
            /// 石狩第1ゾーン
            /// </summary>
            Is1a,
            /// <summary>
            /// 石狩第2ゾーン
            /// </summary>
            Is1b,
            /// <summary>
            /// 東京第1ゾーン
            /// </summary>
            Tk1a,
            /// <summary>
            /// 東京第2ゾーン
            /// </summary>
            Tk1b
        }
        /// <summary>
        /// 設定とロジックを分離するため、ゾーンごとのAPIエンドポイントURLを管理する辞書として分離
        /// </summary>
        private static readonly Dictionary<SakuraCloudZone, string> ZoneUrls = 
            new Dictionary<SakuraCloudZone, string>
            {
                { SakuraCloudZone.Is1a, "https://secure.sakura.ad.jp/cloud/zone/is1a/api/cloud/1.1" },
                { SakuraCloudZone.Is1b, "https://secure.sakura.ad.jp/cloud/zone/is1b/api/cloud/1.1" },
                { SakuraCloudZone.Tk1a, "https://secure.sakura.ad.jp/cloud/zone/tk1a/api/cloud/1.1" },
                { SakuraCloudZone.Tk1b, "https://secure.sakura.ad.jp/cloud/zone/tk1b/api/cloud/1.1" }
            };
        /// <summary>
        /// サクラクラウドのゾーンを指定して SecretManagerClient を初期化します。
        /// Kiotaによる自動生成クラスの上書きを避けつつ、ゾーンごとにAPIエンドポイントURLを自動設定する拡張コンストラクタです。
        /// </summary>
        /// <param name="requestAdapter">リクエストアダプター</param>
        /// <param name="zone">サクラクラウドのゾーン（Is1a, Is1b, Tk1a, Tk1b）</param>
        /// <exception cref="ArgumentOutOfRangeException">未対応のゾーンが指定された場合</exception>
        /// <exception cref="ArgumentNullException">requestAdapterがnullの場合に発生します</exception>
        /// <param name="loggerFactory">The logger factory to use for creating loggers.</param>
        public SecretManagerClient(IRequestAdapter requestAdapter, SakuraCloudZone zone, ILoggerFactory loggerFactory = null)
            : this(PrepareAdapter(requestAdapter, zone), loggerFactory)
        {
            PathParameters["baseurl"] = RequestAdapter.BaseUrl;
        }
        /// <summary>
        /// 指定されたゾーンに対応するAPIエンドポイントURLを取得
        /// コンストラクタの責務外の処理として分離
        /// </summary>
        /// <param name="zone">対象ゾーン</param>
        /// <returns>APIエンドポイントのURL</returns>
        private static string GetUrlForZone(SakuraCloudZone zone)
        {
            if (ZoneUrls.TryGetValue(zone, out var url))
            {
                return url;
            }

            throw new ArgumentOutOfRangeException(
                nameof(zone),
                zone,
                "サポートされていないゾーンです。サポートされているゾーンは: " + string.Join(", ", ZoneUrls.Keys) + ".");
        }
        /// <summary>
        /// アダプターの検証と、ゾーンに基づいたBaseUrlの設定を「コンストラクタ実行前」に実行。
        /// </summary>
        /// <param name="requestAdapter">リクエストアダプター</param>
        /// <param name="zone">サクラクラウドのゾーン</param>
        /// <returns>ゾーンに基づいたBaseUrlの設定済みのリクエストアダプター</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static IRequestAdapter PrepareAdapter(IRequestAdapter requestAdapter, SakuraCloudZone zone)
        {
            // 引数チェック
            if (requestAdapter == null)
            {
                throw new ArgumentNullException(nameof(requestAdapter));
            }

            // ゾーンの妥当性チェックとURL取得（失敗すればここで例外が飛び、thisは実行されない）
            string url = GetUrlForZone(zone);

            // 副作用を最小限にするため、ここでのみ設定を行う
            requestAdapter.BaseUrl = url;

            return requestAdapter;
        }
    }
}
