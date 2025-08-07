# 設計

## BNF

不足はあるかもしれないが、とりあえずこんなような構文となるかと。

```bnf
<expression> ::= <exp_input_operator> <terminator> | <exp_input_progress> <terminator>
<exp_input_operator> ::= <operand> <operator>
<exp_input_progress> ::= <operand> | <exp_input_operator> <operand>
<operand> ::= <digits> | <dot> <digits> | <digits> <dot> | <digits> <dot> <digits>
<digits> ::= <digit> | <digits> <digit>
<terminator> ::= "="
<operator> ::= "+" | "-" | "*" | "/"
<dot> ::= "."
<digit> ::= [0-9]
```

下４つはボタンに対応。それぞれの構成要素もボタン。ボタンを詰めていって式を構成する。

`<state>` に対応する状態を State パターンで作り、個々のstateに応じてDisplay, SubDisplayの表示に対する Strategy として実装する。

```
<state> ::= <expression> | <exp_input_operator> | <exp_input_progress>
```

Strategy は 共通のインターフェースとして定義し、それぞれのDisplayとStateに対応するStragetyを具象クラスとして実装する。Stateが変わると対応するStrategyに変更されるイメージ。

Context は State を持ち、電卓の各ディスプレイに State の変化を通知する Observer パターンを採用する。

`<expression>` の状態になると（つまり`=`が押される）、そのコマンドを**評価**する。すなわち計算が走って計算結果が `<operand>` になるはず。

初期状態は"0"とする。そうすれば NullState の考慮は必要なくなるはず。初期状態は `<exp_input_progress>` となる。

## BackSpaceが押されたときどうするか

上記のようなBNFで入力を実装できていれば、BSボタンを押された際の挙動は、単に入力の配列から末尾をpopするだけでよくなるはず。

strictにデザインパターンに準拠させるのであれば、Mementパターンを使う。数字、演算子ボタンが押されるたびに今の状態をCareTakerに保存し、BSボタンが押されたら Undo で1つ前の状態に戻す。