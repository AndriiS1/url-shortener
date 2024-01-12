import "./about.component.style.css";

export default function About() {
  return (
    <div className="text-wrap">
      <div className="text-container">
        <h1>How generating unique urls works?</h1>
        <span>
          The main part of the algorithm involves this alphabet:
          <b>
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
          </b>
          . Once the algorithm retrieves the user's URL, it begins iterating
          through this string and accumulates a value using the following
          formula:{" "}
          <b>
            aggregate value += (aggregate value x alphabet length) + index of
            the current symbol in alphabet(if there is no symbol in alphabet -1
            will be returned)
          </b>
          . In the end, the absolute value is taken from this code and
          concatenated with the server URL. That's it.
        </span>
        <h3>What about collisions?</h3>
        <span>
          If, after code generation, the system detects that a certain URL is
          already taken, a random number between 0 and 9 is added to the
          original URL. The entire cycle then restarts until a truly unique code
          is created.
        </span>
      </div>
    </div>
  );
}
