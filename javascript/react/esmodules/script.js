import uppercase from "./uppercase.js";
import { a, b as multiply } from "./utils.js";

document.querySelector("#name").innerHTML = `${uppercase(
  "Jonatan"
)}_${a()}: (${multiply(2, 3)})`;
