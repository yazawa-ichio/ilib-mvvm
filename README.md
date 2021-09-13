# ilib-mvvm(開発停止中)
Unity MVVM DataBinding Package

UnityでMVVMでデータバインディングを行うためのパッケージです。  
基本設計は値型を利用する際にボキシングが発生しない目標に実装しています。  

UIのルートにViewコンポーネントを張り、ViewModelをセットした状態で、ViewModelを更新するとデータが反映されます。  
ViewModelのデータを反映するバインダーは独自に実装することが可能です。  
また、オプションを持たない指定のコンポートに対しての単一パラメータのバインディングを行う場合、LightBindという軽量のバインダーを利用できます。  
ViewModelは汎用的に値をセットできるSet/Get関数を利用するものと、プレハブに設定してあるバインダーのパスから自動生成で静的なViewModelを利用するものもあります。  

## [ドキュメント](https://yazawa-ichio.github.io/ilib-unity-project/manual/ilib-mvvm/index.html)

## [APIリファレンス](https://yazawa-ichio.github.io/ilib-unity-project/api/index.html)

# 移行先
https://github.com/yazawa-ichio/UVMBinding  
データバインディングの機能だけが欲しいと自分の中で結論が出たので、その機能だけを持つ別のパッケージに切り出されました。  
今後、こちらのリポジトリで開発を進める予定です。  
