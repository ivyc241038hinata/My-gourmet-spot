package com.example.mygourmetspot

import android.content.Context
import android.content.Intent
import android.os.Bundle
import android.widget.Button
import android.widget.EditText
import android.widget.TextView
import android.widget.Toast
import androidx.appcompat.app.AppCompatActivity

class LoginActivity : AppCompatActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_login)

        val editUserId = findViewById<EditText>(R.id.editUserId)
        val editPassword = findViewById<EditText>(R.id.editPassword)
        val buttonLogin = findViewById<Button>(R.id.buttonLogin)
        val textToRegister = findViewById<TextView>(R.id.textToRegister)

        // 🌟「M」を大文字にして、裏での処理フリーズを防ぐ最も安全な記述に直しました
        editUserId.privateImeOptions = "defaultInputMode=english"

        buttonLogin.setOnClickListener {
            val inputId = editUserId.text.toString().trim()
            val inputPass = editPassword.text.toString().trim()

            val sharedPref = getSharedPreferences("UserAuth", Context.MODE_PRIVATE)
            val savedId = sharedPref.getString("SAVED_ID", null)
            val savedPass = sharedPref.getString("SAVED_PASS", null)

            val isTestAccount = (inputId == "hinataQA1234" && inputPass == "Hina1234:")
            val isRegisteredAccount = (savedId != null && inputId == savedId && inputPass == savedPass)

            if (isTestAccount || isRegisteredAccount) {
                Toast.makeText(this, "ログインしました", Toast.LENGTH_SHORT).show()

                val intent = Intent(this, MainActivity::class.java)
                startActivity(intent)
                finish()
            } else {
                Toast.makeText(this, "IDまたはパスワードが違います", Toast.LENGTH_SHORT).show()
            }
        }

        textToRegister.setOnClickListener {
            val intent = Intent(this, RegisterActivity::class.java)
            startActivity(intent)
        }
    }
}