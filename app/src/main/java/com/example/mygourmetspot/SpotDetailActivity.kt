package com.example.mygourmetspot

import android.content.Context
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import android.view.View
import android.widget.ImageButton // 🌟これを自動インポート（なければ追加）
import android.widget.ImageView
import android.widget.TextView
import android.widget.Toast
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import org.json.JSONArray
import org.json.JSONObject

class SpotDetailActivity : AppCompatActivity() {

    private lateinit var textName: TextView
    private lateinit var textCategory: TextView
    private lateinit var textMemo: TextView
    private lateinit var imagePreview: ImageView
    private lateinit var buttonBack: ImageButton // 🌟戻るボタンの変数を宣言

    private var currentSpotName = ""

    // 🌟 編集画面から戻ってきたときに詳細表示を更新するための仕掛け
    private val editLauncher = registerForActivityResult(ActivityResultContracts.StartActivityForResult()) { result ->
        if (result.resultCode == RESULT_OK) {
            val updatedName = result.data?.getStringExtra("UPDATED_NAME") ?: ""
            if (updatedName.isNotEmpty()) {
                // 画面をリロードするために再読み込み
                loadUpdatedData(updatedName)
                setResult(RESULT_OK) // ホーム画面にも「データが変わったよ」と伝える
            }
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_spot_detail)

        textName = findViewById(R.id.textDetailName)
        textCategory = findViewById(R.id.textDetailCategory)
        textMemo = findViewById(R.id.textDetailMemo)
        imagePreview = findViewById(R.id.imageDetailPreview)
        buttonBack = findViewById(R.id.buttonDetailBack) // 🌟XMLのImageButtonと紐付け
        val buttonEdit = findViewById<TextView>(R.id.textButtonEdit)
        val buttonDelete = findViewById<TextView>(R.id.textButtonDelete)

        // ホーム画面から送られてきたデータを受け取る
        currentSpotName = intent.getStringExtra("SPOT_NAME") ?: ""
        loadUpdatedData(currentSpotName)

        // 🌟 戻るボタンの処理：タップしたらこの画面を閉じてホームに戻る
        buttonBack.setOnClickListener {
            finish()
        }

        // 🌟 編集ボタンの処理：編集画面へお店の名前を持って移動する
        buttonEdit.setOnClickListener {
            val intent = Intent(this, SpotEditActivity::class.java).apply {
                putExtra("SPOT_NAME", currentSpotName)
            }
            editLauncher.launch(intent)
        }

        // 🌟 削除ボタンの処理：設計図通りの「はい／いいえ」ダイアログ
        buttonDelete.setOnClickListener {
            AlertDialog.Builder(this)
                .setTitle("削除の確認")
                .setMessage("本当に削除しますか？")
                .setPositiveButton("はい") { _, _ ->
                    deleteSpotFromStorage(currentSpotName)
                    Toast.makeText(this, "${currentSpotName}を削除しました", Toast.LENGTH_SHORT).show()
                    setResult(RESULT_OK) // リストを再更新するために通知
                    finish() // ホーム画面に戻る
                }
                .setNegativeButton("いいえ", null)
                .show()
        }
    }

    // データをローカル保存（SharedPref）から直接読み込んで表示する関数
    private fun loadUpdatedData(spotName: String) {
        val sharedPref = getSharedPreferences("GourmetLog", Context.MODE_PRIVATE)
        val savedDataStr = sharedPref.getString("SPOT_LIST", "[]") ?: "[]"
        val jsonArray = JSONArray(savedDataStr)

        for (i in 0 until jsonArray.length()) {
            val jsonObject = jsonArray.getJSONObject(i)
            val name = jsonObject.optString("name", "")
            if (name == spotName) {
                currentSpotName = name
                val category = jsonObject.optString("category", "")
                val memo = jsonObject.optString("memo", "")
                val imageUriStr = jsonObject.optString("imageUri", "")

                textName.text = name
                textCategory.text = if (category.isEmpty()) "未分類" else category
                textMemo.text = if (memo.isEmpty()) "メモなし" else memo

                if (imageUriStr.isNotEmpty()) {
                    try {
                        imagePreview.setImageURI(Uri.parse(imageUriStr))
                        imagePreview.visibility = View.VISIBLE
                    } catch (e: Exception) {
                        imagePreview.visibility = View.GONE
                    }
                } else {
                    imagePreview.visibility = View.GONE
                }
                break
            }
        }
    }

    // 🌟 SharedPreferenceから特定のデータを消去する関数
    private fun deleteSpotFromStorage(spotName: String) {
        val sharedPref = getSharedPreferences("GourmetLog", Context.MODE_PRIVATE)
        val savedDataStr = sharedPref.getString("SPOT_LIST", "[]") ?: "[]"
        val jsonArray = JSONArray(savedDataStr)
        val newJsonArray = JSONArray()

        for (i in 0 until jsonArray.length()) {
            val jsonObject = jsonArray.getJSONObject(i)
            if (jsonObject.optString("name", "") != spotName) {
                newJsonArray.put(jsonObject) // 削除対象以外を残す
            }
        }
        sharedPref.edit().putString("SPOT_LIST", newJsonArray.toString()).apply()
    }
}