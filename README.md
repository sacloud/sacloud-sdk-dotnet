# sacloud-sdk-dotnet

[![License](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](LICENSE)

さくらのクラウド .NET SDK

## 概要

sacloud-sdk-dotnetは、[さくらのクラウド](https://cloud.sakura.ad.jp/)の各種サービスを.NET環境から利用するためのSDK群をまとめたモノレポジトリです。

このリポジトリには、さくらのクラウドの各サービスに対応したAPIクライアントライブラリが含まれています。

### APIクライアントライブラリ (`sdk/`)

さくらのクラウドが提供している各サービスのAPIを操作するためのクライアントライブラリ群が入っています。
インストール方法や利用方法に関しては各サービスのREADMEを参照してください。

対応言語 / ランタイムは以下の環境で動くように開発されています。

- C# 7.3 以降
- .NET Standard 2.0 対応

現在サポートしているサービスは以下となります。

- Addon
- API Gateway
- AppRun Dedicated
- AppRun Shared
- CloudHSM
- EventBus
- IAM
- KMS
- Monitoring Suite
- Networking Suite
- Object Storage
- Secret Manager
- SimpleMQ
- Simple Notification
- Workflows

Sharedは各モジュールで共通する処理を担当するモジュールです。

## パッケージの管理

Todo: 管理用スクリプトの例を追加する

## 実装予定

今後実装予定の機能群です。

- IaaS・アプライアンス関連サービス : サーバ・スイッチ等
- 新規サービス・機能群への追従
- プロファイル機能 : https://github.com/sacloud/guide/tree/main/profile

## ライセンス

`sacloud/sacloud-sdk-dotnet` Copyright (C) 2026- The SAKURA internet, Inc.

This project is published under [Apache 2.0 License](LICENSE).
