package com.example.mygourmetspot

import android.app.Activity
import android.content.Context
import android.content.Intent
import android.graphics.Color
import android.net.Uri
import android.os.Bundle
import android.view.View
import android.widget.Button
import android.widget.EditText
import android.widget.ImageView
import android.widget.RatingBar
import android.widget.TextView
import android.widget.Toast
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AppCompatActivity
import org.json.JSONArray

class SpotEditActivity : AppCompatActivity() {

    private lateinit var editName: EditText
    private lateinit var tabCafe: TextView
    private lateinit var tabRamen: TextView
    private lateinit var tabOther: TextView
    private lateinit var buttonPhoto: Button
    private lateinit var imagePreview: ImageView
    private lateinit var ratingBar: RatingBar
    private lateinit var editMemo: EditText
    private lateinit var buttonUpdate: Button
    private lateinit var buttonBack: Button

    private var originalSpotName = ""
    private var selectedCategory = "その他"
    private var selectedImageUri: Uri? = null

    private val pickImageLauncher = registerForActivityResult(ActivityResultContracts.StartActivityForResult()) { result ->
        if (result.resultCode == Activity.RESULT_OK && result.data != null) {
            selectedImageUri = result.data?.data
            if (selectedImageUri != null) {
                imagePreview.setImageURI(selectedImageUri)
                imagePreview.visibility = View.VISIBLE
            }
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_spot_edit)

        editName = findViewById(R.id.editEditName)
        tabCafe = findViewById(R.id.tabCategoryCafe)
        tabRamen = findViewById(R.id.tabCategoryRamen)
        tabOther = findViewById(R.id.tabCategoryOther)
        buttonPhoto = findViewById(R.id.buttonEditPhoto)
        imagePreview = findViewById(R.id.imageEditPreview)
        ratingBar = findViewById(R.id.editRatingBar)
        editMemo = findViewById(R.id.editEditMemo)
        buttonUpdate = findViewById(R.id.buttonUpdate)
        buttonBack = findViewById(R.id.buttonEditBack)

        originalSpotName = intent.getStringExtra("SPOT_NAME") ?: ""

        loadOriginalData()

        tabCafe.setOnClickListener { selectGenre("カフェ") }
        tabRamen.setOnClickListener { selectGenre("ラーメン") }
        tabOther.setOnClickListener { selectGenre("その他") }

        buttonPhoto.setOnClickListener {
            val intent = Intent(Intent.ACTION_OPEN_DOCUMENT).apply {
                addCategory(Intent.CATEGORY_OPENABLE)
                type = "image/*"
            }
            pickImageLauncher.launch(intent)
        }

        buttonBack.setOnClickListener {
            finish()
        }

        buttonUpdate.setOnClickListener {
            val newName = editName.text.toString().trim()
            val newMemo = editMemo.text.toString().trim()
            val newRating = ratingBar.rating.toInt()
            val newImageUriStr = selectedImageUri?.toString() ?: ""

            if (newName.isEmpty()) {
                Toast.makeText(this, "お店の名前を入力してください", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            updateSpotInStorage(newName, selectedCategory, newMemo, newRating, newImageUriStr)

            Toast.makeText(this, "更新しました！", Toast.LENGTH_SHORT).show()

            val resultIntent = Intent().apply {
                putExtra("UPDATED_NAME", newName)
            }
            setResult(RESULT_OK, resultIntent)
            finish()
        }
    }

    private fun loadOriginalData() {
        val sharedPref = getSharedPreferences("GourmetLog", Context.MODE_PRIVATE)
        val savedDataStr = sharedPref.getString("SPOT_LIST", "[]") ?: "[]"
        val jsonArray = JSONArray(savedDataStr)

        for (i in 0 until jsonArray.length()) {
            val jsonObject = jsonArray.getJSONObject(i)
            if (jsonObject.optString("name", "") == originalSpotName) {
                val name = jsonObject.optString("name", "")
                val category = jsonObject.optString("category", "")
                val memo = jsonObject.optString("memo", "")
                val rating = jsonObject.optInt("rating", 3)
                val imageUriStr = jsonObject.optString("imageUri", "")

                editName.setText(name)
                editMemo.setText(memo)
                ratingBar.rating = rating.toFloat()
                selectGenre(category)

                if (imageUriStr.isNotEmpty()) {
                    selectedImageUri = Uri.parse(imageUriStr)
                    imagePreview.setImageURI(selectedImageUri)
                    imagePreview.visibility = View.VISIBLE
                }
                break
            }
        }
    }

    private fun selectGenre(category: String) {
        selectedCategory = category
        tabCafe.setBackgroundColor(Color.WHITE)
        tabCafe.setTextColor(Color.BLACK)
        tabRamen.setBackgroundColor(Color.WHITE)
        tabRamen.setTextColor(Color.BLACK)
        tabOther.setBackgroundColor(Color.WHITE)
        tabOther.setTextColor(Color.BLACK)

        when (category) {
            "カフェ" -> {
                tabCafe.setBackgroundColor(Color.BLACK)
                tabCafe.setTextColor(Color.WHITE)
            }
            "ラーメン" -> {
                tabRamen.setBackgroundColor(Color.BLACK)
                tabRamen.setTextColor(Color.WHITE)
            }
            else -> {
                tabOther.setBackgroundColor(Color.BLACK)
                tabOther.setTextColor(Color.WHITE)
            }
        }
    }

    // 🌟 編集時は createdAt が無ければ固定補填し、updatedAt は必ず最新の現在時刻に上書き！
    private fun updateSpotInStorage(name: String, category: String, memo: String, rating: Int, imageUriStr: String) {
        val sharedPref = getSharedPreferences("GourmetLog", Context.MODE_PRIVATE)
        val savedDataStr = sharedPref.getString("SPOT_LIST", "[]") ?: "[]"
        val jsonArray = JSONArray(savedDataStr)

        val now = System.currentTimeMillis()

        for (i in 0 until jsonArray.length()) {
            val jsonObject = jsonArray.getJSONObject(i)
            if (jsonObject.optString("name", "") == originalSpotName) {
                jsonObject.put("name", name)
                jsonObject.put("category", category)
                jsonObject.put("memo", memo)
                jsonObject.put("rating", rating)

                // 既存の createdAt がなければ作成
                if (!jsonObject.has("createdAt")) {
                    jsonObject.put("createdAt", 1704067200000L + ((jsonArray.length() - i) * 1000L))
                }

                // 🌟 更新日時を最新（現在時刻）にする
                jsonObject.put("updatedAt", now)

                if (imageUriStr.isNotEmpty()) {
                    jsonObject.put("imageUri", imageUriStr)
                }
                break
            }
        }
        sharedPref.edit().putString("SPOT_LIST", jsonArray.toString()).apply()
    }
}