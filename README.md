# DentakuWebApp

C# 製のシンプルなコンソール電卓です。四則演算（足し算・引き算・かけ算・割り算）に対応し、メニュー形式で操作できます。

## 必要環境
- .NET 8.0 SDK 以降

## 使い方
1. リポジトリのルートで依存関係を復元し、ビルドします。
   ```bash
   dotnet restore
   dotnet build
   ```
2. アプリを実行します。
   ```bash
   dotnet run
   ```
3. 表示されるメニューから操作を選び、数値を入力してください。

## WASM での実行方法
コンソールアプリをブラウザー上で動かす場合は WebAssembly 用に発行した後、静的ファイルとしてホスティングします。

1. WebAssembly のビルドツールをインストールします（初回のみ）。
   ```bash
   dotnet workload install wasm-tools
   ```
2. WASM 向けに発行します。`WASM` シンボルを定義してブラウザー用の入出力コードを有効にします。
   ```bash
   dotnet publish -c Release -r browser-wasm --self-contained true -p:DefineConstants=WASM
   ```
3. 発行物 (`bin/Release/net8.0/browser-wasm/AppBundle/`) を静的サーバーで配信します。
   - .NET CLI がある場合: `dotnet serve --directory bin/Release/net8.0/browser-wasm/AppBundle --port 8000`
   - もしくは Python があれば: `python3 -m http.server 8000 --directory bin/Release/net8.0/browser-wasm/AppBundle`
4. ブラウザーで `http://localhost:8000` にアクセスすると、コンソール入出力付きの電卓が動作します。

## GitHub Pages での確認方法
GitHub Pages でホストする場合は、発行した `AppBundle` をそのまま配置するだけです。リポジトリ名に合わせてベースパスを設定すると 404 を防げます。

1. GitHub Pages でのホスティング用に発行します。リポジトリ名が `DentakuWebApp` である前提で、アセットのベースパスを `/DentakuWebApp/` に設定しています（別名の場合は置き換えてください）。
   ```bash
   dotnet publish -c Release -r browser-wasm --self-contained true -p:DefineConstants=WASM -p:WasmRelativeAssetBasePath=/DentakuWebApp/
   ```
2. 発行された `bin/Release/net8.0/browser-wasm/AppBundle/` の中身を、Pages の公開ディレクトリ（例: `docs/` または `gh-pages` ブランチのルート）にコピーします。
3. GitHub の設定で、公開ディレクトリを選択して Pages を有効化します。
4. 数分後に `https://<GitHubユーザー名>.github.io/<リポジトリ名>/` でブラウザー実行版を確認できます。

## 主な機能
- メニューで操作を選択し、数値を2つ入力するだけで計算結果を表示
- 無効な入力（数値以外、メニュー外の番号）に対する再入力要求
- 0 で割ろうとした際のエラーメッセージ表示
