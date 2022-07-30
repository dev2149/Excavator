using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text;
using System.Text.RegularExpressions;

public class LoginSystem : MonoBehaviour
{
    public string loginUrl = "http://YourWebsite.com/.../Login.php";
    public string registerURL = "http://YourWebsite.com/.../Register.php";
    public string checUsernameUrl = "http://YourWebsite.com/.../CheckUsername.php";
    public string checEmailUrl = "http://YourWebsite.com/.../CheckEmail.php";
    public string forgotUrl = "http://YourWebsite.com/.../Forgot.php";
    public string newPasswordURL = "http://YourWebsite.com/.../NewPassword.php";

    public string secretGameKey = "12345";
    public string secretServerKey = "54321";

    private string resetEmail;
    private string resetCode;

    void Start()
    {
        if (PlayerPrefs.GetInt("LoginRemember") == 1)
        {
            GameObject _go = GameObject.Find("Canvas").transform.Find("Login").gameObject;
            _go.transform.Find("Username").gameObject.GetComponent<InputField>().text = PlayerPrefs.GetString("LoginUsername");
            _go.transform.Find("Password").gameObject.GetComponent<InputField>().text = PlayerPrefs.GetString("LoginPassword");
            _go.transform.Find("Remember Me").gameObject.GetComponent<Toggle>().isOn = true;
        }
    }

    public void Login(GameObject _go)
    {
        string _username = _go.transform.Find("Username").gameObject.GetComponent<InputField>().text;
        string _password = _go.transform.Find("Password").gameObject.GetComponent<InputField>().text;
        string _key = Md5Sum(_username + secretGameKey).ToLower();

        _go.transform.Find("Username").gameObject.GetComponent<InputField>().enabled = false;
        _go.transform.Find("Password").gameObject.GetComponent<InputField>().enabled = false;
        _go.transform.Find("Remember Me").gameObject.GetComponent<Toggle>().enabled = false;
        _go.transform.Find("Login").gameObject.GetComponent<Button>().enabled = false;
        _go.transform.Find("Register").gameObject.GetComponent<Button>().enabled = false;
        _go.transform.Find("Forgot").gameObject.GetComponent<Button>().enabled = false;

        _go.transform.Find("Msg").GetComponent<Text>().text = "...";
        StartCoroutine(LoginIE(_username, _password, _key, _go));
    }
    IEnumerator LoginIE(string _username, string _password, string _key, GameObject _go)
    {
        WWWForm form = new WWWForm();
        form.AddField("name", _username);
        form.AddField("pass", Md5Sum(_password));
        form.AddField("key", _key);

        WWW www = new WWW(loginUrl, form);
        yield return www;
        string _return = www.text;
        string _returnKey = Md5Sum(_username + secretServerKey).ToLower();

        if (_return == "")
        {
            _go.transform.Find("Msg").GetComponent<Text>().text = "Server offline";
        }
        else if (_return == _returnKey)
        {
            _go.transform.Find("Msg").GetComponent<Text>().text = "Welcome";
            if (_go.transform.Find("Remember Me").gameObject.GetComponent<Toggle>().isOn)
            {
                PlayerPrefs.SetInt("LoginRemember", 1);
                PlayerPrefs.SetString("LoginUsername", _username);
                PlayerPrefs.SetString("LoginPassword", _password);
            }
            else
            {
                PlayerPrefs.SetInt("LoginRemember", 0);
            }

            GameObject _goUsername = new GameObject("Username: " + _username);
            DontDestroyOnLoad(_goUsername);
            Application.LoadLevel("Game");
        }
        else
        {
            _go.transform.Find("Msg").GetComponent<Text>().text = "Incorrect password or username";
            _go.transform.Find("Username").gameObject.GetComponent<InputField>().enabled = true;
            _go.transform.Find("Password").gameObject.GetComponent<InputField>().enabled = true;
            _go.transform.Find("Remember Me").gameObject.GetComponent<Toggle>().enabled = true;
            _go.transform.Find("Login").gameObject.GetComponent<Button>().enabled = true;
            _go.transform.Find("Register").gameObject.GetComponent<Button>().enabled = true;
            _go.transform.Find("Forgot").gameObject.GetComponent<Button>().enabled = true;
        }
    }

    public void ResetRegister(GameObject _go)
    {
        InputField _username = _go.transform.Find("Username").gameObject.GetComponent<InputField>();
        InputField _password = _go.transform.Find("Password").gameObject.GetComponent<InputField>();
        InputField _passwordComfirm = _go.transform.Find("Password Comfirm").gameObject.GetComponent<InputField>();
        InputField _email = _go.transform.Find("Email").gameObject.GetComponent<InputField>();
        InputField _emailComfirm = _go.transform.Find("Email Comfirm").gameObject.GetComponent<InputField>();
        Toggle _termsToggle = _go.transform.Find("Toggle").gameObject.GetComponent<Toggle>();
        Button _terms = _go.transform.Find("Terms").gameObject.GetComponent<Button>();
        Text _msg = _go.transform.Find("Msg").gameObject.GetComponent<Text>();

        _username.text = "";
        _password.text = "";
        _passwordComfirm.text = "";
        _email.text = "";
        _emailComfirm.text = "";
        _termsToggle.isOn = false;
        _msg.text = "";

        _username.enabled = true;
        _password.enabled = true;
        _passwordComfirm.enabled = true;
        _email.enabled = true;
        _emailComfirm.enabled = true;
        _termsToggle.enabled = true;
        _go.transform.Find("Back").gameObject.GetComponent<Button>().enabled = true;
        _go.transform.Find("Submit").gameObject.GetComponent<Button>().enabled = true;
    }
    public void Register(GameObject _go)
    {
        StartCoroutine(RegisterIE(_go));
    }
    IEnumerator RegisterIE(GameObject _go)
    {
        InputField _username = _go.transform.Find("Username").gameObject.GetComponent<InputField>();
        InputField _password = _go.transform.Find("Password").gameObject.GetComponent<InputField>();
        InputField _passwordComfirm = _go.transform.Find("Password Comfirm").gameObject.GetComponent<InputField>();
        InputField _email = _go.transform.Find("Email").gameObject.GetComponent<InputField>();
        InputField _emailComfirm = _go.transform.Find("Email Comfirm").gameObject.GetComponent<InputField>();
        Toggle _termsToggle = _go.transform.Find("Toggle").gameObject.GetComponent<Toggle>();
        Button _terms = _go.transform.Find("Terms").gameObject.GetComponent<Button>();
        Text _msg = _go.transform.Find("Msg").gameObject.GetComponent<Text>();

        _go.transform.Find("Back").gameObject.GetComponent<Button>().enabled = false;
        _go.transform.Find("Submit").gameObject.GetComponent<Button>().enabled = false;

        _username.enabled = false;
        _password.enabled = false;
        _passwordComfirm.enabled = false;
        _email.enabled = false;
        _emailComfirm.enabled = false;
        _termsToggle.enabled = false;
        _terms.enabled = false;
        _msg.text = "";

        bool _error = false;
        if (_username.text == "")
        {
            _username.image.color = Color.red;
            _msg.text = "Enter username";
            _error = true;
        }
        else
        {
            WWWForm form = new WWWForm();
            form.AddField("name", _username.text);

            WWW www = new WWW(checUsernameUrl, form);
            yield return www;
            string _return = www.text;

            if (_return == "")
            {
                _username.image.color = Color.white;
                _msg.text = "Server offline";
                _error = true;
            }
            else if (_return == "false")
            {
                _username.image.color = Color.red;
                _msg.text = "Username already in use";
                _error = true;
            }
            else
            {
                _username.image.color = Color.white;
            }
        }

        if (!_error)
        {
            if (_password.text == "")
            {
                _password.image.color = Color.red;
                _msg.text = "Enter password";
                _error = true;
            }
            else
            {
                _password.image.color = Color.white;

                if (_passwordComfirm.text != _password.text)
                {
                    _passwordComfirm.image.color = Color.red;
                    _msg.text = "Passwords does not match";
                    _error = true;
                }
                else
                {
                    _passwordComfirm.image.color = Color.white;
                }
            }
        }

        if (!_error)
        {
            if (_email.text == "")
            {
                _msg.text = "Enter email";
                _error = true;
            }
            else
            {
                if (!VerifyEmailAddress(_email.text))
                {
                    _email.image.color = Color.red;
                    _msg.text = "Email not valid";
                    _error = true;
                }
                else
                {
                    _email.image.color = Color.white;

                    if (_emailComfirm.text != _email.text)
                    {
                        _emailComfirm.image.color = Color.red;
                        _msg.text = "Emails does not match";
                        _error = true;
                    }
                    else
                    {
                        WWWForm form = new WWWForm();
                        form.AddField("email", _email.text);

                        WWW www = new WWW(checEmailUrl, form);
                        yield return www;
                        string _return = www.text;

                        if (_return == "false")
                        {
                            _email.image.color = Color.red;
                            _msg.text = "Email already in use";
                            _error = true;
                        }
                        else
                        {
                            _email.image.color = Color.white;
                        }
                    }
                }
            }
        }

        if (!_error)
        {
            if (!_termsToggle.isOn)
            {
                _termsToggle.image.color = Color.red;
                _error = true;
            }
            else
            {
                _termsToggle.image.color = Color.white;
            }
        }

        if (!_error)
        {
            WWWForm form = new WWWForm();
            form.AddField("name", _username.text);
            form.AddField("pass", Md5Sum(_password.text));
            form.AddField("email", _email.text);
            form.AddField("key", Md5Sum(_username.text + secretGameKey).ToLower());

            WWW www = new WWW(registerURL, form);
            yield return www;
            string _return = www.text;
            string _returnKey = Md5Sum(_username.text + secretServerKey).ToLower();

            if (_returnKey == _return)
            {
                _msg.text = "Registration is complete";
                yield return new WaitForSeconds(1);
                Application.LoadLevel(Application.loadedLevel);
            }
            else
            {
                _msg.text = "Server offline";
                _error = true;
            }
        }

        if (_error)
        {
            _username.enabled = true;
            _password.enabled = true;
            _passwordComfirm.enabled = true;
            _email.enabled = true;
            _emailComfirm.enabled = true;
            _termsToggle.enabled = true;
            _terms.enabled = true;
            _go.transform.Find("Back").gameObject.GetComponent<Button>().enabled = true;
            _go.transform.Find("Submit").gameObject.GetComponent<Button>().enabled = true;
        }
    }

    public void Forgot(GameObject _go)
    {
        StartCoroutine(ForgotIE(_go));
    }
    IEnumerator ForgotIE(GameObject _go)
    {
        InputField _email = _go.transform.Find("Email").gameObject.GetComponent<InputField>();
        Text _msg = _go.transform.Find("Msg").gameObject.GetComponent<Text>();

        Button _back = _go.transform.Find("Back").gameObject.GetComponent<Button>();
        Button _send = _go.transform.Find("Send").gameObject.GetComponent<Button>();

        if (_email.text == "")
        {
            _msg.text = "Enter email";
        }
        else
        {
            if (!VerifyEmailAddress(_email.text))
            {
                _msg.text = "Email not valid";
            }
            else
            {
                _back.enabled = false;
                _send.enabled = false;
                _email.enabled = false;

                WWWForm form = new WWWForm();
                form.AddField("email", _email.text);

                WWW www = new WWW(forgotUrl, form);
                yield return www;
                string _return = www.text;

                if (_return == "")
                {
                    _msg.text = "Server offline";
                    _back.enabled = true;
                    _send.enabled = true;
                    _email.enabled = true;
                }
                else
                {
                    _msg.text = "Reset code has been sent to your email";

                    resetCode = _return;
                    resetEmail = _email.text;

                    yield return new WaitForSeconds(1);

                    _back.enabled = true;
                    _send.enabled = true;
                    _email.enabled = true;
                    _email.text = "";

                    _go.transform.parent.Find("ResetCode").gameObject.SetActive(true);
                    _go.SetActive(false);
                }
            }
        }
    }

    public void ResetCode(GameObject _go)
    {
        InputField _code = _go.transform.Find("Code").gameObject.GetComponent<InputField>();
        Text _msg = _go.transform.Find("Msg").gameObject.GetComponent<Text>();

        if (_code.text != resetCode)
        {
            _msg.text = "Reset code not valid";
        }
        else
        {
            _code.text = "";
            _go.transform.parent.Find("NewPassword").gameObject.SetActive(true);
            _go.SetActive(false);
        }
    }

    public void NewPassword(GameObject _go)
    {
        StartCoroutine(NewPasswordIE(_go));
    }
    IEnumerator NewPasswordIE(GameObject _go)
    {
        InputField _password = _go.transform.Find("Password").gameObject.GetComponent<InputField>();
        InputField _passwordComfirm = _go.transform.Find("Password Comfirm").gameObject.GetComponent<InputField>();

        Text _msg = _go.transform.Find("Msg").gameObject.GetComponent<Text>();


        if (_password.text == "")
        {
            _msg.text = "Enter new password";
        }
        else
        {
            if (_passwordComfirm.text != _password.text)
            {
                _msg.text = "Passwords does not match";
            }
            else
            {
                WWWForm form = new WWWForm();
                form.AddField("email", resetEmail);
                form.AddField("pass", Md5Sum(_password.text));
                form.AddField("key", Md5Sum(resetEmail + secretGameKey).ToLower());

                WWW www = new WWW(newPasswordURL, form);
                yield return www;
                string _return = www.text;

                if (_return == "")
                {
                    _msg.text = "Server offline";
                }
                else
                {
                    _msg.text = "Your password has been changed";
                }

                yield return new WaitForSeconds(1);
                _go.transform.parent.Find("Login").gameObject.SetActive(true);
                _go.SetActive(false);
            }
        }
    }


    public bool VerifyEmailAddress(string address)
    {
        string[] atCharacter;
        string[] dotCharacter;

        atCharacter = address.Split("@"[0]);
        if (atCharacter.Length == 2)
        {
            dotCharacter = atCharacter[1].Split("."[0]);
            if (dotCharacter.Length >= 2)
            {
                if (dotCharacter[dotCharacter.Length - 1].Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public string Md5Sum(string _data)
    {
        // step 1, calculate MD5 hash from input
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(_data);
        byte[] hash = md5.ComputeHash(inputBytes);

        // step 2, convert byte array to hex string
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }
}
