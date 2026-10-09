package com.example.mygourmetspot

import android.content.Context
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity

class RegisterActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_register)

        val editNewUserId = findViewById<EditText>(R.id.editNewUserId)
        val editNewPassword = findViewById<EditText>(R.id.editNewPassword)
        val buttonRegister = findViewById<Button>(R.id.buttonRegister)
        val textToLogin = findViewById<TextView>(R.id.textToLogin)

        buttonRegister.setOnClickListener {
            val userId = editNewUserId.text.toString().trim()
            val password = editNewPassword.text.toString().trim()

            if (userId.isEmpty() || password.isEmpty()) {
                Toast.makeText(this, "IDとパスワードを入力してください", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            if (password.length < 6) {
                Toast.makeText(this, "パスワードは6文字以上で入力してください", Toast.LENGTH_SHORT).show()
                return@setOnClickListener
            }

            val sharedPref = getSharedPreferences("UserAuth", Context.MODE_PRIVATE)
            with(sharedPref.edit()) {
                putString("SAVED_ID", userId)
                putString("SAVED_PASS", password)
                apply()
            }

            Toast.makeText(this, "アカウントを作成しました！", Toast.LENGTH_SHORT).show()
            finish()
        }

        textToLogin.setOnClickListener {
            finish()
        }
    }
}