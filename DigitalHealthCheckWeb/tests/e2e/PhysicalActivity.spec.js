describe("Physical Activity page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/PhysicalActivity';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Physical activity page 1 of 4");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/PhysicalActivity')).toBeTruthy();
    })

    it("goes to GPPAQ1 if mostly sitting selected ", async () => {
          // Click text=Mostly sitting
        await page.click('text=Mostly sitting');
        const checked = await page.isChecked('#work-activity', 'sitting');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ1')).toBeTruthy();
        await expect(page).toMatchText("h1", "Physical activity page 2 of 4");
 
    }) 

    it("goes to GPPAQ1 if mostly standing or walking selected ", async () => {
        await page.click('#work-activity-2', 'standing');
        const checked = await page.isChecked('#work-activity-2', 'standing');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ1')).toBeTruthy();
        await expect(page).toMatchText("h1", "Physical activity page 2 of 4");
 
    }) 

    it("goes to GPPAQ1 if definite physical activity selected ", async () => {
        // Click text=Definite physical activity
      await page.click('text=Definite physical activity');
      const checked = await page.isChecked('#work-activity-3', 'definite');
      expect(checked).toBeTruthy();
      await page.click('text=Continue');
      expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ1')).toBeTruthy();
      await expect(page).toMatchText("h1", "Physical activity page 2 of 4");

  }) 

it("goes to GPPAQ1 if vigorous physical activity selected ", async () => {
    // Click text=Vigorous physical activity
  await page.click('text=Vigorous physical activity');
  const checked = await page.isChecked('#work-activity-4', 'vigorous');
  expect(checked).toBeTruthy();
  await page.click('text=Continue');
  expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ1')).toBeTruthy();
  await expect(page).toMatchText("h1", "Physical activity page 2 of 4");

})

it("goes to GPPAQ1 if not in employment selected ", async () => {
    // Click text=I am not in employment
  await page.click('text=I am not in employment');
  const checked = await page.isChecked('#work-activity-6', 'unemployed');
  expect(checked).toBeTruthy();
  await page.click('text=Continue');
  expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/GPPAQ1')).toBeTruthy();
  await expect(page).toMatchText("h1", "Physical activity page 2 of 4");

})

it("should have a back link", async () => {
  // Click text=Back
  await page.click('text=Back');
  expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/DoYouDrinkAlcohol')).toBeTruthy();
})

})