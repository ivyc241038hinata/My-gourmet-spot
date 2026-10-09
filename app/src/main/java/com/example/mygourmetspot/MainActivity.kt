package com.example.mygourmetspot

import android.app.Activity
import android.content.Context
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import android.text.Editable
import android.text.TextWatcher
import android.view.LayoutInflater
import android.view.View
import android.widget.Button
import android.widget.EditText
import android.widget.ImageView
import android.widget.LinearLayout
import android.widget.RatingBar
import android.widget.TextView
import android.widget.Toast
import androidx.activity.enableEdgeToEdge
import androidx.activity.result.contract.ActivityResultContracts
import androidx.appcompat.app.AlertDialog
import androidx.appcompat.app.AppCompatActivity
import org.json.JSONArray
import org.json.JSONObject

data class GourmetSpot(
    val name: String,
    val category: String,
    val memo: String,
    val rating: Int,
    val imageUriStr: String,
    val createdAt: Long = System.currentTimeMillis(),
    var updatedAt: Long = System.currentTimeMillis()
)

class MainActivity : AppCompatActivity() {

    private var selectedImageUri: Uri? = null
    private var currentDialogImageView: ImageView? = null
    private lateinit var spotListContainer: LinearLayout

    private val allSpots = mutableListOf<GourmetSpot>()

    private var currentSearchKeyword = ""
    private var currentSortBy = "RATING"
    private var currentSelectedGenres = listOf("すべて")
    private var currentSelectedRatings = listOf<Int>()

    private val detailLauncher = registerForActivityResult(ActivityResultContracts.StartActivityForResult()) { result ->
        if (result.resultCode == Activity.RESULT_OK) {
            loadSpotsFromStorage()
        }
    }

    private val pickImageLauncher = registerForActivityResult(ActivityResultContracts.StartActivityForResult()) { result ->
        if (result.resultCode == Activity.RESULT_OK && result.data != null) {
            selectedImageUri = result.data?.data
            if (selectedImageUri != null && currentDialogImageView != null) {
                currentDialogImageView?.setImageURI(selectedImageUri)
                currentDialogImageView?.visibility = View.VISIBLE
            }
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContentView(R.layout.activity_main)

        spotListContainer = findViewById(R.id.spotListContainer)
        val buttonAddSpot = findViewById<Button>(R.id.buttonAddSpot)
        val editSearch = findViewById<EditText>(R.id.editSearch)
        val buttonFilter = findViewById<Button>(R.id.buttonFilter)
        val buttonLogout = findViewById<TextView>(R.id.textButtonLogout)

        try {
            loadSpotsFromStorage()
        } catch (e: Exception) {
            e.printStackTrace()
        }

        buttonLogout.setOnClickListener {
            AlertDialog.Builder(this)
                .setTitle("ログアウトの確認")
                .setMessage("ログアウトしてログイン画面に戻りますか？")
                .setPositiveButton("はい") { _, _ ->
                    val intent = Intent(this, LoginActivity::class.java).apply {
                        flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                    }
                    startActivity(intent)
                    finish()
                }
                .setNegativeButton("いいえ", null)
                .show()
        }

        editSearch.addTextChangedListener(object : TextWatcher {
            override fun beforeTextChanged(s: CharSequence?, start: Int, count: Int, after: Int) {}
            override fun onTextChanged(s: CharSequence?, start: Int, before: Int, count: Int) {
                currentSearchKeyword = s.toString().trim()
                applyFilterAndSort()
            }
            override fun afterTextChanged(s: Editable?) {}
        })

        buttonFilter.setOnClickListener {
            val filterSheet = FilterBottomSheetFragment()

            filterSheet.setOnFilterApplyListener(object : FilterBottomSheetFragment.OnFilterApplyListener {
                override fun onApply(p0: String, p1: List<String>, p2: List<Int>) {
                    currentSortBy = p0
                    currentSelectedGenres = p1
                    currentSelectedRatings = p2
                    applyFilterAndSort()
                }
            })

            filterSheet.show(supportFragmentManager, "FilterSheet")
        }

        buttonAddSpot.setOnClickListener {
            val ctx = this
            val dialogView = LayoutInflater.from(ctx).inflate(R.layout.dialog_add_spot, null)

            val editName = dialogView.findViewById<EditText>(R.id.editDialogName)
            val editCategory = dialogView.findViewById<EditText>(R.id.editDialogCategory)
            val editMemo = dialogView.findViewById<EditText>(R.id.editDialogMemo)
            val buttonAddPhoto = dialogView.findViewById<Button>(R.id.buttonAddPhoto)
            val imagePreview = dialogView.findViewById<ImageView>(R.id.imagePreview)
            val ratingBar = dialogView.findViewById<RatingBar>(R.id.dialogRatingBar)

            currentDialogImageView = imagePreview
            selectedImageUri = null

            buttonAddPhoto.setOnClickListener {
                val intent = Intent(Intent.ACTION_OPEN_DOCUMENT).apply {
                    addCategory(Intent.CATEGORY_OPENABLE)
                    type = "image/*"
                }
                pickImageLauncher.launch(intent)
            }

            val builder = AlertDialog.Builder(ctx)
            builder.setTitle("新規スポット登録")
            builder.setView(dialogView)

            builder.setPositiveButton("登録") { dialog, _ ->
                val name = editName.text.toString().trim()
                val category = editCategory.text.toString().trim()
                val memo = editMemo.text.toString().trim()
                val ratingValue = ratingBar.rating.toInt()
                val imageUriStr = selectedImageUri?.toString() ?: ""

                if (name.isEmpty()) {
                    Toast.makeText(ctx, "お店の名前を入力してください", Toast.LENGTH_SHORT).show()
                    return@setPositiveButton
                }

                val now = System.currentTimeMillis()
                val newSpot = GourmetSpot(name, category, memo, ratingValue, imageUriStr, now, now)
                allSpots.add(0, newSpot)

                saveAllSpotsToStorage()
                applyFilterAndSort()

                Toast.makeText(ctx, "追加しました！", Toast.LENGTH_SHORT).show()
                currentDialogImageView = null
                selectedImageUri = null
                dialog.dismiss()
            }

            builder.setNegativeButton("キャンセル") { dialog, _ ->
                currentDialogImageView = null
                selectedImageUri = null
                dialog.dismiss()
            }
            builder.show()
        }
    }

    // 🌟 詳細・編集画面から戻ってきた時に自動で最新データを読み直して画面更新する
    override fun onResume() {
        super.onResume()
        try {
            loadSpotsFromStorage()
        } catch (e: Exception) {
            e.printStackTrace()
        }
    }

    private fun applyFilterAndSort() {
        spotListContainer.removeAllViews()

        val filteredList = allSpots.filter { spot ->
            val matchKeyword = currentSearchKeyword.isEmpty() ||
                    spot.name.contains(currentSearchKeyword, ignoreCase = true) ||
                    spot.category.contains(currentSearchKeyword, ignoreCase = true) ||
                    spot.memo.contains(currentSearchKeyword, ignoreCase = true)

            val standardGenres = listOf("カフェ", "ラーメン", "居酒屋", "スーパー")

            val matchGenre = currentSelectedGenres.contains("すべて") ||
                    currentSelectedGenres.contains(spot.category) ||
                    (currentSelectedGenres.contains("その他") && !standardGenres.contains(spot.category))

            val matchRating = currentSelectedRatings.isEmpty() ||
                    currentSelectedRatings.contains(spot.rating)

            matchKeyword && matchGenre && matchRating
        }

        // 🌟 登録日時・更新日時の降順（大きい値＝最新）で並べ替え
        val sortedList = when (currentSortBy) {
            "NEWEST" -> filteredList.sortedByDescending { it.createdAt }
            "UPDATED" -> filteredList.sortedByDescending { it.updatedAt }
            else -> filteredList.sortedByDescending { it.rating }
        }

        for (spot in sortedList) {
            addSpotViewToLayout(spot)
        }
    }

    private fun addSpotViewToLayout(spot: GourmetSpot) {
        val itemView = LayoutInflater.from(this).inflate(R.layout.list_item_spot, null)

        itemView.findViewById<TextView>(R.id.textListName).text = spot.name
        itemView.findViewById<TextView>(R.id.textListCategory).text = if(spot.category.isEmpty()) "未分類" else spot.category
        itemView.findViewById<RatingBar>(R.id.listRatingBar).rating = spot.rating.toFloat()
        itemView.findViewById<TextView>(R.id.textListRatingNumber).text = "全体評価 ${spot.rating}"

        val textMemo = itemView.findViewById<TextView>(R.id.textListMemo)
        textMemo.text = if (spot.memo.isEmpty()) "メモなし" else "📝 ${spot.memo}"

        val listImageView = itemView.findViewById<ImageView>(R.id.imageListPhoto)
        if (spot.imageUriStr.isNotEmpty()) {
            try {
                val uri = Uri.parse(spot.imageUriStr)
                try {
                    contentResolver.takePersistableUriPermission(uri, Intent.FLAG_GRANT_READ_URI_PERMISSION)
                } catch (pe: SecurityException) {
                    pe.printStackTrace()
                }
                listImageView.setImageURI(uri)
                listImageView.visibility = View.VISIBLE
            } catch (e: Exception) {
                listImageView.visibility = View.GONE
            }
        } else {
            listImageView.visibility = View.GONE
        }

        itemView.setOnClickListener {
            val intent = Intent(this, SpotDetailActivity::class.java).apply {
                putExtra("SPOT_NAME", spot.name)
            }
            detailLauncher.launch(intent)
        }

        spotListContainer.addView(itemView)
    }

    private fun saveAllSpotsToStorage() {
        val sharedPref = getSharedPreferences("GourmetLog", Context.MODE_PRIVATE)
        val jsonArray = JSONArray()
        for (spot in allSpots) {
            val jsonObject = JSONObject().apply {
                put("name", spot.name)
                put("category", spot.category)
                put("memo", spot.memo)
                put("rating", spot.rating)
                put("imageUri", spot.imageUriStr)
                put("createdAt", spot.createdAt)
                put("updatedAt", spot.updatedAt)
            }
            jsonArray.put(jsonObject)
        }
        sharedPref.edit().putString("SPOT_LIST", jsonArray.toString()).apply()
    }

    private fun loadSpotsFromStorage() {
        val sharedPref = getSharedPreferences("GourmetLog", Context.MODE_PRIVATE)
        val savedDataStr = sharedPref.getString("SPOT_LIST", "[]") ?: "[]"

        val jsonArray = JSONArray(savedDataStr)
        allSpots.clear()

        // 🌟 補填用タイムスタンプのベースを固定（過去の基準時間：2024年1月1日）
        val baseFixedTime = 1704067200000L

        for (i in 0 until jsonArray.length()) {
            val jsonObject = jsonArray.getJSONObject(i)

            // 古いデータにはインデックスの逆順で固定時間を割り当てる（毎回値が変わらないようにする）
            val dummyTime = baseFixedTime + ((jsonArray.length() - i) * 1000L)

            val createdAtVal = if (jsonObject.has("createdAt")) jsonObject.getLong("createdAt") else dummyTime
            val updatedAtVal = if (jsonObject.has("updatedAt")) jsonObject.getLong("updatedAt") else createdAtVal

            val spot = GourmetSpot(
                name = jsonObject.optString("name", ""),
                category = jsonObject.optString("category", ""),
                memo = jsonObject.optString("memo", ""),
                rating = jsonObject.optInt("rating", 3),
                imageUriStr = jsonObject.optString("imageUri", ""),
                createdAt = createdAtVal,
                updatedAt = updatedAtVal
            )
            allSpots.add(spot)
        }
        applyFilterAndSort()
    }
}