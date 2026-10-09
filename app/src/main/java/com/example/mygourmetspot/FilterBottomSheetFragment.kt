package com.example.mygourmetspot

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.CheckBox
import android.widget.RadioGroup
import android.widget.TextView
import com.google.android.material.bottomsheet.BottomSheetDialogFragment
import com.google.android.material.chip.Chip

class FilterBottomSheetFragment : BottomSheetDialogFragment() {

    interface OnFilterApplyListener {
        fun onApply(
            sortBy: String,
            selectedGenres: List<String>,
            selectedRatings: List<Int>
        )
    }

    private var listener: OnFilterApplyListener? = null

    fun setOnFilterApplyListener(listener: OnFilterApplyListener) {
        this.listener = listener
    }

    override fun onCreateView(
        inflater: LayoutInflater,
        container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        // 🌟 39行目：Android Studioの補足文字をすべて排除した、最も安全な記述です
        return inflater.inflate(R.layout.dialog_filter_spots, container, false)
    }

    override fun onViewCreated(view: View, savedInstanceState: Bundle?) {
        super.onViewCreated(view, savedInstanceState)

        val textClose = view.findViewById<TextView>(R.id.textClose)
        val radioGroupSort = view.findViewById<RadioGroup>(R.id.radioGroupSort)

        val checkGenreAll = view.findViewById<CheckBox>(R.id.checkGenreAll)
        val checkGenreCafe = view.findViewById<CheckBox>(R.id.checkGenreCafe)
        val checkGenreRamen = view.findViewById<CheckBox>(R.id.checkGenreRamen)
        val checkGenreIzakaya = view.findViewById<CheckBox>(R.id.checkGenreIzakaya)
        val checkGenreSuper = view.findViewById<CheckBox>(R.id.checkGenreSuper)
        val checkGenreOther = view.findViewById<CheckBox>(R.id.checkGenreOther)

        val chipStar1 = view.findViewById<Chip>(R.id.chipStar1)
        val chipStar2 = view.findViewById<Chip>(R.id.chipStar2)
        val chipStar3 = view.findViewById<Chip>(R.id.chipStar3)
        val chipStar4 = view.findViewById<Chip>(R.id.chipStar4)
        val chipStar5 = view.findViewById<Chip>(R.id.chipStar5)

        val buttonReset = view.findViewById<Button>(R.id.buttonReset)
        val buttonApply = view.findViewById<Button>(R.id.buttonApply)

        textClose.setOnClickListener { dismiss() }

        checkGenreAll.setOnClickListener {
            if (checkGenreAll.isChecked) {
                checkGenreCafe.isChecked = false
                checkGenreRamen.isChecked = false
                checkGenreIzakaya.isChecked = false
                checkGenreSuper.isChecked = false
                checkGenreOther.isChecked = false
            }
        }

        val otherGenres = listOf(checkGenreCafe, checkGenreRamen, checkGenreIzakaya, checkGenreSuper, checkGenreOther)
        for (checkbox in otherGenres) {
            checkbox.setOnClickListener {
                if (checkbox.isChecked) {
                    checkGenreAll.isChecked = false
                }
            }
        }

        buttonReset.setOnClickListener {
            radioGroupSort.check(R.id.radioSortRating)
            checkGenreAll.isChecked = true
            checkGenreCafe.isChecked = false
            checkGenreRamen.isChecked = false
            checkGenreIzakaya.isChecked = false
            checkGenreSuper.isChecked = false
            checkGenreOther.isChecked = false
            chipStar1.isChecked = false
            chipStar2.isChecked = false
            chipStar3.isChecked = false
            chipStar4.isChecked = false
            chipStar5.isChecked = false
        }

        buttonApply.setOnClickListener {
            val sortBy = when (radioGroupSort.checkedRadioButtonId) {
                R.id.radioSortNewest -> "NEWEST"
                R.id.radioSortUpdated -> "UPDATED"
                else -> "RATING"
            }

            val genres = mutableListOf<String>()
            if (checkGenreAll.isChecked) genres.add("すべて")
            if (checkGenreCafe.isChecked) genres.add("カフェ")
            if (checkGenreRamen.isChecked) genres.add("ラーメン")
            if (checkGenreIzakaya.isChecked) genres.add("居酒屋")
            if (checkGenreSuper.isChecked) genres.add("スーパー")
            if (checkGenreOther.isChecked) genres.add("その他")

            val ratings = mutableListOf<Int>()
            if (chipStar1.isChecked) ratings.add(1)
            if (chipStar2.isChecked) ratings.add(2)
            if (chipStar3.isChecked) ratings.add(3)
            if (chipStar4.isChecked) ratings.add(4)
            if (chipStar5.isChecked) ratings.add(5)

            listener?.onApply(sortBy, genres, ratings)
            dismiss()
        }
    }
}