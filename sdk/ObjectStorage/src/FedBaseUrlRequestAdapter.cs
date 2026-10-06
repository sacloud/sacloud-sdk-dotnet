using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions.Store;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Sacloud.Sdk.ObjectStorage
{
    /// <summary>
    /// サイト未指定で利用するために採用している固定ベース URL（/fed/v1）を適用するリクエストアダプター。
    /// デコレータパターンで既存の <see cref="IRequestAdapter"/> をラップし、
    /// すべてのリクエストのベース URL を、採用している固定ベース URL（/fed/v1）に差し替えます。
    /// </summary>
    public sealed class FedBaseUrlRequestAdapter : IRequestAdapter
    {
        private readonly IRequestAdapter _innerRequestAdapter;
        private const string FedBaseUrl = "https://secure.sakura.ad.jp/cloud/zone/is1a/api/objectstorage/1.0/fed/v1";

        /// <summary>
        /// <see cref="FedBaseUrlRequestAdapter"/> の新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="innerRequestAdapter">ラップ対象の <see cref="IRequestAdapter"/></param>
        /// <exception cref="ArgumentNullException"><paramref name="innerRequestAdapter"/> が null の場合に発生します</exception>
        public FedBaseUrlRequestAdapter(IRequestAdapter innerRequestAdapter)
        {
            _innerRequestAdapter = innerRequestAdapter ?? throw new ArgumentNullException(nameof(innerRequestAdapter));
        }

        /// <inheritdoc />
        public string BaseUrl
        {
            get { return FedBaseUrl; }
            set { }
        }

        /// <inheritdoc />
        public ISerializationWriterFactory SerializationWriterFactory => _innerRequestAdapter.SerializationWriterFactory;

        /// <inheritdoc />
        public void EnableBackingStore(IBackingStoreFactory backingStoreFactory)
            => _innerRequestAdapter.EnableBackingStore(backingStoreFactory);

        /// <summary>
        /// RequestInformation のベース URL を、採用している固定ベース URL（/fed/v1）に書き換える
        /// </summary>
        /// <param name="requestInfo">リクエスト情報</param>
        /// <returns>ベースURL書き換え後のリクエスト情報</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private RequestInformation OverrideBaseUrl(RequestInformation requestInfo)
        {
            if (requestInfo == null) throw new ArgumentNullException(nameof(requestInfo));

            // requestInfo.PathParameters に含まれるベース URL を、採用している固定ベース URL（/fed/v1）に書き換える
            requestInfo.PathParameters["baseurl"] = FedBaseUrl;

            // requestInfo.PathParameters["baseurl"] の書き換えだけでは、 HttpClientRequestAdapter.SetBaseUrlForRequestInformation() メソッドにより
            // その値が元々のベース URL に再度書き換えられるため、 requestInfo.URI を直接指定することでベース URL の書き換えを確定させる
            // <see cref="https://github.com/microsoft/kiota-http-dotnet/blob/0d09ba10bf29c880d92919e36e82f6b65c089fc3/src/HttpClientRequestAdapter.cs#L554-L557" />
            var fixedUri = requestInfo.URI;
            requestInfo.URI = fixedUri;
            return requestInfo;
        }

        /// <inheritdoc />
        public Task<ModelType> SendAsync<ModelType>(
            RequestInformation requestInfo,
            ParsableFactory<ModelType> factory,
            Dictionary<string, ParsableFactory<IParsable>> errorMapping = null,
            CancellationToken cancellationToken = default)
            where ModelType : IParsable
            => _innerRequestAdapter.SendAsync(OverrideBaseUrl(requestInfo), factory, errorMapping, cancellationToken);

        /// <inheritdoc />
        public Task<IEnumerable<ModelType>> SendCollectionAsync<ModelType>(
            RequestInformation requestInfo,
            ParsableFactory<ModelType> factory,
            Dictionary<string, ParsableFactory<IParsable>> errorMapping = null,
            CancellationToken cancellationToken = default)
            where ModelType : IParsable
            => _innerRequestAdapter.SendCollectionAsync(OverrideBaseUrl(requestInfo), factory, errorMapping, cancellationToken);

        /// <inheritdoc />
        public Task<ModelType> SendPrimitiveAsync<ModelType>(
            RequestInformation requestInfo,
            Dictionary<string, ParsableFactory<IParsable>> errorMapping = null,
            CancellationToken cancellationToken = default)
            => _innerRequestAdapter.SendPrimitiveAsync<ModelType>(OverrideBaseUrl(requestInfo), errorMapping, cancellationToken);

        /// <inheritdoc />
        public Task<IEnumerable<ModelType>> SendPrimitiveCollectionAsync<ModelType>(
            RequestInformation requestInfo,
            Dictionary<string, ParsableFactory<IParsable>> errorMapping = null,
            CancellationToken cancellationToken = default)
            => _innerRequestAdapter.SendPrimitiveCollectionAsync<ModelType>(OverrideBaseUrl(requestInfo), errorMapping, cancellationToken);

        /// <inheritdoc />
        public Task SendNoContentAsync(
            RequestInformation requestInfo,
            Dictionary<string, ParsableFactory<IParsable>> errorMapping = null,
            CancellationToken cancellationToken = default)
            => _innerRequestAdapter.SendNoContentAsync(OverrideBaseUrl(requestInfo), errorMapping, cancellationToken);

        /// <inheritdoc />
        public Task<T> ConvertToNativeRequestAsync<T>(
            RequestInformation requestInfo,
            CancellationToken cancellationToken = default)
            => _innerRequestAdapter.ConvertToNativeRequestAsync<T>(OverrideBaseUrl(requestInfo), cancellationToken);
    }
}