# How Richie designs

This explains how the Richie Design Program designs screens, for the people and agents who design the next area (account settings first). It is drawn from the identity (auth) canvas, **Richie Identity** (Claude Design artifact `9EbW44Zt3zAyXumY4m4Rr2`, 27 boards plus `richie.css`, read on 2026-11-04). That canvas is unfinished. Each section separates **what we do** (what the canvas shows) from **what we should do better**.

Product facts every design has to respect: accounts are personal. A user can join any number of workspaces but can create only one. Sign-in is passwordless: an 8-character email code (Crockford Base32, valid for 5 minutes, 4 tries, at least 1 minute between sends) or a passkey. The brand mark is Richie, a pixel shiba.

---

## 1. Pipeline

**What we do**
1. **Design** in a Claude Design canvas (artifact type "Design"), starting from suiss blocks (https://suiss.app/ui). The identity boards are visibly built on the suiss `verify-code`, `login-passkey` and `signup-form` blocks: a centred column, an icon above a 4xl/5xl headline, a large muted lede, rounded OTP slots and a resend timer. They are then re-skinned for Richie: the lock icon became the shiba, the "›" chevrons were dropped, the code grew from 6 digits to 8 characters, and auto-submit and the shake animation were removed.
2. **Test while designing.** Sub-agents render pages at every resolution and hunt for bugs. The canvas shows the results of this loop rather than reports: CSS comments record contrast fixes, and the layout has rules against things moving between states.
3. **Evaluate.** One independent agent reviews the UI and UX and gives ship or no-ship for each design, published as an artifact.

**What we should do better**
- Keep the test and evaluation outputs next to the canvas. Link them from a canvas note or a `docs/design/<area>/` file. Today the reasons only survive as CSS comments.
- Give the evaluator a fixed checklist (§6) and a fixed harness (§8), so verdicts can be compared across areas.

---

## 2. Principles (inferred from the work)

1. **One task per screen.** Each screen has one filled primary button, and secondary actions are outline or plain. Every screen also has a way out: "use a different email", "Email me a code instead", "Not now", "Not you?".
2. **States don't move the page.** Content is anchored to the top, and the headline stays the same across states ("Check your email" is kept for the error, expired and locked states). Space below the code is reserved (`.rc-code-meta` has a `min-height`). The comment in `richie.css` reads: *"switching state never moves the mascot, the headline or the button a person is about to tap."*
3. **Say what happened and what to do next, with real numbers.** For example "You have 3 tries left", "Resend code in 0:59", "Codes expire after 5 minutes."
4. **Accessible as drawn.** Boards use real `<button>`, `<a>`, `<input>` and `<label>` elements, `aria-describedby` from fields to their errors, live regions, `aria-disabled` (which keeps focus) instead of `disabled`, and visually hidden labels. They include rules for `forced-colors` and `prefers-reduced-motion`. Contrast is computed, not guessed.
5. **Inputs work with the platform.** They set `autocomplete` (`given-name`, `family-name`, `email`, `username webauthn`, `one-time-code`), `inputmode`, `autocapitalize` and `enterkeyhint`. The OTP is one real input laid over eight drawn slots, so pasting and autofill work.
6. **Deviate from the system only for a measured reason, and write the reason down.** Each departure from suiss is justified by a contrast ratio in a CSS comment.
7. **The brand lives in one place.** The shiba is the only brand element, drawn pixel-exact. Everything else is the neutral suiss look.
8. **Reassure in plain words.** For example "Richie never sees your face or fingerprint." The account chip keeps the email domain visible, so a work account is easy to tell from a personal one.

---

## 3. Visual language

### Source of truth
| Layer | Where | Notes |
|---|---|---|
| Design tokens (app) | `frontend/packages/ui/src/styles/globals.css` | The suiss theme in oklch, shadcn `base-nova`, `iconLibrary: phosphor`, `--radius: 0.625rem` |
| Canvas stylesheet | `project/richie.css` in the canvas | Hex values, `rc-` prefixed recipes, about 16 KB. Every board links it |
| Brand mark | Canvas asset `/_blob/6af6329655ac2e226c3e26d79e124698` (31×39 PNG) | `assets/richie.png` in the repo is a 1254×1254 square master, a different file |

`richie.css` reproduces the suiss tokens exactly in light mode. I converted every oklch value to hex to check. It departs from suiss in these places:

| Token | suiss | richie.css | Why |
|---|---|---|---|
| Dark `--primary` | `#2997ff` | stays `#0071e3` | White on `#2997ff` is 3.02:1 |
| Primary hover | `primary/80` (lighter) | `#0068d6` (darker) | Lighter would drop below 4.5:1 |
| Focus ring | `ring-3 ring/50` | 2px solid `--focus` plus a 3px halo (`--ring`) | Visible on black |
| Dark `--muted` | `#161617` | `#1d1d1f` | **Undocumented** |
| Chip link (light) | `#0071e3` | `#0066cc` | `#0071e3` on `#f5f5f7` is 4.31:1 |
| Body size | Tailwind default, 16px | 17px / 1.47 | Apple-style body text |

### Colour (light / dark)
| Role | Light | Dark |
|---|---|---|
| bg / fg | `#ffffff` / `#1d1d1f` | `#000000` / `#f5f5f7` |
| muted / muted-fg | `#f5f5f7` / `#6e6e73` | `#1d1d1f` / `#a1a1a6` |
| border / input | `#d2d2d7` / `#86868b` | `#424245` / `#6e6e73` |
| primary / link / focus | `#0071e3` | `#0071e3` / `#2997ff` / `#2997ff` |
| danger / danger-bg | `#d0000a` / 5% tint | `#ff5447` / 10% tint |
| success | `#1e8e3e` | (no dark value) |

Measured contrast: body text 16.8:1 light and 19.3:1 dark; muted text 5.07 and 8.16; links 4.70 and 6.96; white on the primary button 4.70; danger on its tint 5.20 and 6.11; the input border 3.62 and 4.14. **Weak pairs:** the dimmed primary button is 2.09:1 (disabled controls are exempt, but it is the main action on the locked screen); the outline button's edge is 1.51:1 in light and 2.10:1 in dark; disabled OTP slot edges are 1.38:1 and 1.68:1.

### Type
Font stack: `-apple-system, BlinkMacSystemFont, "Inter Variable", "Inter", "Segoe UI", Roboto, sans-serif`. Inter is loaded from Google Fonts on every board but only renders off Apple devices.

| Use | Size / line height / weight |
|---|---|
| Title | 34 / 1.12 / 600, -0.022em. 40px at ≥768w and ≥640h. 48px at ≥768w and ≥860h. 26px at ≤540h |
| Lede | 17 / 1.47, or 19 / 1.42 on desktop, muted |
| Body, input, button | 17 |
| Switch line | 15 |
| Label | 14 / 1.3 / 600 |
| Hint, error, "or" | 13 / 1.4 |
| Legal, note, footer | 12 / 1.5 |
| OTP glyph | `clamp(20px, 6.2cqi, 28px)` / 500, tabular, uppercase |

### Shape and spacing
- **Radii:** 12px for inputs, OTP slots and mobile buttons. Desktop buttons and the chip are pills (999px). The avatar and the mascot badge are circles.
- **Column widths:** column max 460px, lede max 400, OTP row max 442, note max 380.
- **Page padding:** top `max(32px, 8vh)`, then `max(48px, 10vh)` and `max(64px, 12vh)` on larger screens. Sides 20px (16px below 360px wide, 24px on desktop).
- **Gaps:** form 20, row 12, field 6, actions 12. Spacing is chosen by eye (6, 10, 22, 28…) rather than on a strict 4/8 grid.

### Component recipes (`rc-*`)
- **Input:** 52px tall, 1px `--input` border, 12px radius. Focus adds a solid ring and a soft halo. Invalid uses a danger border and a 5% danger tint, plus an error line with a circle-! icon linked by `aria-describedby`. The label always sits above the field. The placeholder is only an example (`you@example.com`).
- **Buttons:** `--primary` (filled), `--outline`, `--plain` (link-coloured text), `--wide` (min 280px). On desktop they are pills (min 240×44). At ≤480px they are full width, 50px tall, with a 12px radius. Busy is a spinner plus a gerund ending in "…", with `aria-disabled`. Unavailable is `.is-unavailable` at opacity 0.5.
- **Links:** always underlined ("colour alone does not separate them from the grey text"). Small links get a larger hit area through an invisible `::after` (about 44px).
- **OTP:** one input over 4 + 4 slots with a separator. The slots scale with container query units, not breakpoints. Classes: `is-active`, `is-invalid`, `is-disabled`, and `is-focused` (canvas-only; it draws the arrival focus).
- **Chip:** an avatar with initials plus the email split into a local part, which truncates first, and a domain, which stays visible, plus a "Not you?" link.
- **Mascot badge:** a green check or a red "!" circle on the shiba for success and failure screens.
- **Icons:** hand-drawn inline SVGs (a key, circle-!, a check), not phosphor.

---

## 4. Layout, brand, density

**What we do**
- One centred column on a plain page: no card, no border, no illustration. A footer with "Help · Privacy · Terms" sits at the bottom. The column header is centred, and form fields are aligned to the start.
- **The shiba** sits above every headline at exact multiples of its 31×39 grid: 2× on mobile, 3× on desktop, 1× on short screens. It uses `image-rendering: pixelated` and `alt="Richie"`. A state badge is the only thing ever added to it.
- **Density is low.** The headline dominates, with about five elements per screen and generous empty space. This suits a page seen once per session.
- **Responsiveness is fluid.** A 2-column name row collapses below 360px. Buttons go full width at ≤480px. The type scale depends on viewport **height** as well as width.

**What we should do better**
- The height-based type scale makes taller windows look worse. At 1280×1000 the sign-up title grows to 48px and wraps to two lines; at 1280×800 it is 40px on one line.
- On desktop the pill button (240px, centred) sits under full-width 52px inputs. The widths and heights don't line up (44px button against 52px inputs).
- The switch line is a flex row with wrap, so `text-wrap: balance` cannot balance it. At 375px "…spam folder, or" ends one line and the link sits alone on the next.

---

## 5. Copy voice

### Rules (as practised)
- **Sentence case** everywhere. Only proper names are title case ("Terms of Service", "Privacy Policy").
- **Address the user as "you", and Richie as "we".** Use contractions (we’ll, can’t, isn’t, didn’t, couldn’t), curly apostrophes and a real ellipsis (…).
- **Errors tell the user what to do, not what they did wrong.** They are imperative and give an example or a count, never "invalid", "oops" or an exclamation mark:
  - "Enter your first name."
  - "Enter a full email address, like name@example.com."
  - "That code isn’t right. You have 3 tries left."
  - "This code has expired. Send a new one to continue."
  - "That was the last try for this code. Send a new one to continue."
- **Waits name the thing being waited on:** "Checking code…", "Waiting for your device…", "Follow the prompt on your screen to finish.", "Resend code in 0:59", "You can send a new code in 0:27".
- **Failures explain the likely causes and the next step:** "The request was cancelled or timed out, or this device has no passkey for Richie. Try again to use one from your phone."
- **Headlines are short:** "Create your Richie account", "Sign in to Richie", "Check your email", "Welcome back, Jamie", "Couldn’t sign you in", "Sign in faster next time", "Passkey created".
- **Ledes carry the product model and the benefit:** "One account for all your workspaces.", "Sign in with your passkey. No code needed.", "No code to wait for."
- **Button vocabulary:** Continue · Sign in with a passkey · Create a passkey · Try again · Send a new code · Resend code · Email me a code instead · Not now · Continue to Richie. Escape links: Sign in · Create an account · use a different email · Not you?
- **Use digits for numbers and m:ss for countdowns.** Lists have no Oxford comma ("face, fingerprint or screen lock").
- **Screen-reader text is specific:** "Verification code, 8 letters and numbers", "Checking your code…", "You can resend the code now."

### Inconsistencies to fix
- "Resend code" and "Send a new code" are used for what is, to the user, the same action. Pick one verb.
- The placeholder says `you@example.com` but the error says `name@example.com`.
- "cancelled" (British) alongside US-default copy. Pick a locale.
- Failure headlines mix two forms: "Couldn’t sign you in" (a sentence) and "Passkey not created" (a label).
- "Passkey not created" repeats the offer's lede instead of saying what happened, and drops the privacy note.
- Document `<title>`s read like board names ("Sign in to Richie, check your email"). That one also collides with Verify's headline.
- Expired and locked keep "Enter the code we sent to…" above a disabled input, so the instruction no longer applies.

---

## 6. State coverage

**Covered in identity (27 boards):** default; field errors (one or two at once); checking (busy); wrong code with tries left; expired; locked out with a cooldown; resend available; waiting for device; device failed or cancelled; created (success); not created; returning user (personalised); mobile; dark.

**Missing from identity:** submitting the email form (busy); resend confirmation ("We sent a new code"; the live region exists but is never filled); server or network error; enumeration-safe copy for an unknown email or an email that is already registered; send rate limit; a browser without passkey support; 1 try left; desktop renders of any state (states are drawn only at 390px); dark versions of any error (dark danger tokens never appear on the canvas); visible keyboard focus; long names and emails; short or landscape screens; forced colours; an invite or workspace context.

**Checklist for every new area (account settings included).** Draw each item as a board, or note why it is not needed:
- [ ] Default with realistic data · empty (for example no passkeys) · long or extreme data (long names, emails, many workspaces)
- [ ] Initial loading · saving (busy button) · saved (inline confirmation and a polite announcement)
- [ ] Field validation · server error · offline · rate limit or cooldown
- [ ] Destructive confirmation · step-up re-authentication (code or passkey) for sensitive changes
- [ ] Device waiting, failed or cancelled (passkeys) · not allowed (role or ownership)
- [ ] Widths 320, 375/390, 768, 1280, 1440 · short height (≤540) · **dark for every state** · forced colours · reduced motion · visible focus · 200% zoom

---

## 7. Canvas conventions

### What we do
- **`canvas.json`:** `"v": 3`, title "Richie Identity", `"launch": {"view": "canvas"}`, `"pages": []`, `"designSystems": []`, and an explicit `order`.
- **Rows are flows, columns are variants.** Rows are 1280px apart (y = 0, 1280, 2560, 3840, 5120). Each row starts with the desktop base (1440×900 at x = 0), then 390×844 boards from x = 1520 at a 470px pitch: mobile, mobile dark, then states. Each row has one title note 260px above it (`"kind": "title1"`, `maxW` = the row's span), for example "Verify email code (sign-up and sign-in)". There are no other notes.
- **File names:** `<Flow>[-<state>].dc.html`, for example `Verify-expired`, `Passkey-signin-waiting`, `Signin-dark`. The sign-up base is `Main.dc.html`.
- **Board titles:** "<Flow>, <variant>", for example "Verify code, no tries left" and "Sign in, mobile dark".
- **`expand: "fill"`** is set on the desktop bases and all state boards, so they fill the window as pages. It is not set on the fixed 390px mobile and dark wrappers. No board sets `is_interactive`, but boards do link to each other (`href="Signin.dc.html"`).
- **How variants reuse a base:**
  - *Mobile and dark* are thin wrappers that import the desktop base:
    ```html
    <div class="dark" style="width: 390px; min-height: 844px; background: #000000">
      <dc-import name="Signin" hint-size="390px,844px"></dc-import>
    </div>
    ```
    The mobile wrapper is the same without `class="dark"` or the background. The dark wrapper's helmet also sets `body{margin:0;background:#000}`.
  - *States* are **full copies** of the base markup with local edits: 12 state copies, which with the 5 bases makes 17 boards of full markup. I checked them against each other and the footer and mascot markup has not drifted yet.
- **How a board gets its CSS and runtime:** these lines are identical on every board.
  ```html
  <head> …
  <script src="./support.js"></script>
  <link rel="stylesheet" href="./richie.css">
  </head>
  <body><x-dc>
  <helmet>
  <link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Inter:wght@400..700&amp;display=swap">
  <style>
  body{margin:0}
  </style>
  </helmet>
  <div class="rc"> … </div>
  </x-dc>
  <script type="text/x-dc" data-dc-script data-props='{"$preview":{"width":1440,"height":900}}'>
  class Component extends DCLogic { renderVals() { return {}; } }
  </script>
  ```
  - The shared stylesheet is linked from `<head>`, outside `<helmet>`. I did not check how the canvas itself renders that link.
  - `$preview` is 1440×900 on desktop bases and 390×844 on everything else.
  - `richie.css` sets `x-dc { display: contents; }`, so the raw file also renders in a plain browser.
- **The boards use no runtime features:** no `{{holes}}`, `<sc-if>`, `<sc-for>`, Tweaks, or state. All copy is literal markup, which `format.md` recommends ("Tweaks are levers, not copy").
- **Theme switching:** dark is the `.dark` class (`.rc.dark` or `.dark .rc`), and `html:has(.dark .rc)` paints the root black. There is no `prefers-color-scheme` rule.

### What we should do better
- **Name the base after its screen** (`Signup`, not `Main`). Use `<Area>-<screen>[-<state>][-<theme>]` consistently.
- **Stop copying state boards.** Use one base with a `state` enum Tweak and `<sc-if>` branches, or `<dc-import>` the shared chrome with props. Seventeen copies means one footer edit touches 17 files.
- **Mark boards that link to each other as `is_interactive`.** Add a `pages` or `launch: focused` entry point so a reviewer can click through the flow.
- **Add behaviour notes to each row:** timers, the try limit, when the passkey sign-in shows instead of the email sign-in, and where "Not now" goes. Today these rules are only implied by the copy.
- **Attach the design system** (`designSystems`), or link `globals.css`, so the hex/oklch mapping is explicit rather than duplicated.
- **Draw at least one desktop render and one dark render of each error state.**

---

## 8. How boards are tested

**The render harness works.** Files live in `.design/wp2-1/harness/`:
1. Read the boards with the Artifact tool (`action: "read"`, `paths: [...]`). Read each image by its blob hash (`path: "6af6329655ac2e226c3e26d79e124698"`), which saves `<hash>.png`.
2. Run `node .design/wp2-1/harness/build-boards.mjs <project-dir> <out-dir> <blob-dir>`. For each board it:
   - takes the content of `<x-dc>`;
   - moves the `<helmet>` children into `<head>`;
   - replaces each `<dc-import name="X">` with X's body, recursively, keeping the wrapper's `dark` class but dropping its fixed 390px width;
   - rewrites `/_blob/<hash>` to `blobs/<hash>.png`;
   - links `richie.css`, drops `support.js` and the logic script, and rewrites links between boards from `X.dc.html` to `X.html`.

   All 27 boards build.
3. Serve the output over HTTP, because playwright-cli blocks `file:` URLs: `python3 -I -m http.server <port> --bind 127.0.0.1 --directory <out-dir>`. Pick a free port; another session already held 8765.
4. Open it with `playwright-cli -s=<name> open --browser=chromium http://127.0.0.1:<port>/Main.html`. The default `chrome` channel is not installed.
5. Run `playwright-cli -s=<name> --raw run-code --filename=.design/wp2-1/harness/shoot.js`. It loops over boards × viewports, takes full-page screenshots, and writes a JSON report with horizontal overflow, title size and targets under 44px. The base URL and output folder are hard-coded at the top of the script.

**Results** for 7 boards × {320×640, 375×812, 1280×800, 1280×1000}, in `.design/wp2-1/shots/`:
- No horizontal overflow anywhere.
- The 8 OTP slots fit at 320px.
- Dark mode renders correctly through the inlined import.
- Inline links measure 22px tall, but `::after` extends their hit area to about 44px.
- The legal links are 15px tall with no extension. The inline-text exemption applies, but they are small.
- Footer links are 26px tall on a fine pointer, and padded to 44px or more only on `(pointer: coarse)`.

**Limits and caveats:**
- Boards that use holes, `sc-if`, `sc-for` or state need the canvas runtime (`support.js`); the harness would render their raw template text.
- `is-focused` draws a focus ring that is not real focus.
- Fonts differ by platform: SF on macOS, Inter on Linux CI. Pin a font if screenshots must match across machines.
- Google Fonts needs network access.
- Test several **heights** as well as widths, because the type scale switches on height.

---

## 9. Known gaps and what not to carry into account settings

**Unfinished or wrong in identity:**
- "use a different email" always goes to sign-up (`Main.dc.html`), even from the sign-in flow.
- On the locked screen, the primary button is dimmed to 2.09:1, and the error says "Send a new one" while the button is unavailable. Show the countdown in the button instead.
- Error states have no error summary, no change to the document title, and no rule for where focus goes on submit. Some passkey headlines get `tabindex="-1"` as a focus target; Verify's do not.
- `aria-live="assertive"` wraps a static hint, and a second polite region sits beside it, so screen readers may announce twice.
- `alt="Richie"` announces the brand before every headline. Use `alt=""` where the headline already names Richie.
- `success` has no dark token.
- The icons are hand-drawn, but the repo declares phosphor.
- The look is the suiss / Apple-store palette almost unchanged (#1d1d1f, #f5f5f7, #0071e3, 17px/1.47). Apart from the shiba, nothing distinguishes it, so the identity rests entirely on the mascot.

**For account settings:**
- **Carry:** the tokens and the deviations justified by contrast; visible labels with hints and errors below; `aria-describedby`; `aria-disabled` for busy controls; busy labels as a gerund plus "…"; copy that says what happened and what to do next; a way out on every flow; layouts where states don't move the page; always-underlined links; autocomplete tokens; one primary action per section; the account chip pattern (domain stays visible); the reassurance tone.
- **Reuse directly:** changing your email goes through the Verify flow with the same timers, tries and copy. Name fields match sign-up. Passkey management is required: the "not created" screen already promises "You can also add a passkey later in your account settings."
- **Do not copy:**
  - the centred single column, the 34–48px headline and a mascot on every page. Settings is a dense area people come back to: use the app shell and the suiss settings blocks;
  - the height-based type scale;
  - full-copy state boards;
  - dark mode as a class only. Settings likely owns the Appearance preference, so design System / Light / Dark and `prefers-color-scheme`;
  - opacity-only disabled buttons;
  - `rc-*` CSS as the implementation. Map the recipes onto `@repo/ui` (shadcn + suiss) components.
- **Draw before shipping:**
  - step-up re-authentication for email change, passkey removal and account deletion;
  - removing your last passkey (email code remains as a fallback);
  - leaving a workspace, and the constraint that you can own only one workspace;
  - saved, saving and error states;
  - the full §6 checklist in both themes.
