using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// C#で簡単なstateパターンを書いてみるテスト
/// </summary>
namespace statePatturn
{
    /// <summary>
    /// ステートの番号
    /// </summary>
    enum EState {
        EState_leftState,
        EState_rightState
    }

    /// <summary>
    /// ステート管理のクラス。
    /// 実際に使用する際はこのクラスを継承して使う
    /// </summary>
    public class Context<T>
    {
        // ステートリスト
        public List<IState<T>> StateList = new List<IState<T>>();
        // 現在のステート
        public IState<T> CurrentState;

        /// <summary>
        /// 現在のステートを実行
        /// </summary>
        public void ExecuteState()
        {
            // ジェネリックを使って自分自身を渡す場合は、いったんobject型に変換したうえで指定の型に変換する
            T context = (T)(object)this;
            CurrentState.Execute(context);
        } 

        /// <summary>
        /// ステートの切り替え、およびステートが切り替えられた直後に実行する処理を実行
        /// </summary>
        public void ChangeState(IState<T> setState)
        {
            T context = (T)(object)this;
            CurrentState = setState;
            CurrentState.Enter(context);
        }
    }

    // 上記、Contextを継承した管理クラス
    public class Character : Context<Character> {

        // ステート共通のパラメータ
        // 実際は量が多いので別のオブジェクトにまとめるはず
        public int posX = 0;

        /// <summary>
        /// コンストラクタ
        /// 使用するステートクラスをここで先に生成しておき、最初のステートを設定する
        /// (unityで複数のオブジェクトにアタッチされることを考慮して、シングルトンを使用せずに実装)
        /// </summary>
        public Character(){
            StateList.Add(new leftState());
            StateList.Add(new rightState());

            ChangeState(StateList[(int)EState.EState_leftState]);
        }
    }

    /// <summary>
    ///  ステートインターフェース
    /// </summary>
    public interface IState<T>
    {
        /// <summary>
        /// ステートが切り替えられた直後に実行
        /// </summary>
        void Enter(T context);

        /// <summary>
        /// 処理実行
        /// </summary>
        void Execute(T context);
    }

    /// <summary>
    /// ステートインターフェースを継承したステートクラスその１
    /// </summary>
    class leftState : IState<Character>
    {
        public void Enter(Character context)
        {
            
        }
        public void Execute(Character context)
        {
            context.posX--;
            Console.WriteLine("左に移動中:{0}", context.posX);
            if (context.posX <= -5)
            {
                context.ChangeState(context.StateList[(int)EState.EState_rightState]);
            }
        }        
    }

    /// <summary>
    /// ステートインターフェースを継承したステートクラスその２
    /// </summary>
    class rightState : IState<Character>
    {
        public void Enter(Character context)
        {

        }
        public void Execute(Character context)
        {
            context.posX++;
            Console.WriteLine("右に移動中:{0}", context.posX);
            if (context.posX >= 5)
            {
                context.ChangeState(context.StateList[(int)EState.EState_leftState]);
            }
        }
    }

    /// <summary>
    /// メイン処理
    /// ループでステート管理クラスを呼び出している
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var Character = new Character();

            for (int i = 0; i < 20; i++)
            {
                Character.ExecuteState();
            }
        }
    }
}
